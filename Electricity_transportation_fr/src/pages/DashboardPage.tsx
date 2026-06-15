import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { generationApi } from '../api/generationApi';

export const DashboardPage = () => {
    const navigate = useNavigate();
    
    const [plants, setPlants] = useState<any[]>([]);
    const [loading, setLoading] = useState(true);


    // Стан для Модалки подачі прогнозу
    const [forecastModal, setForecastModal] = useState<{plantId: number, maxCapacity: number} | null>(null);
    const [forecastDate, setForecastDate] = useState(new Date().toISOString().split('T')[0]);
    // Масив на 24 години, заповнений нулями
    const [hourlyKw, setHourlyKw] = useState<number[]>(Array(24).fill(0));

    // Стан для Модалки історії
    const [historyModal, setHistoryModal] = useState<any[] | null>(null);

    const [historyTab, setHistoryTab] = useState<'verified' | 'pending'>('verified');

    useEffect(() => {
        fetchPlants();
    }, []);

    const fetchPlants = async () => {
        try {
            setLoading(true);
            const data = await generationApi.getPlants();
            setPlants(data);
        } catch (err: any) {
            alert(err.response?.data?.error || 'Помилка завантаження станцій');
        } finally {
            setLoading(false);
        }
    };

    const handleLogout = () => {
        localStorage.removeItem('token');
        navigate('/auth');
    };  

    const handleStatusChange = async (plantId: number, currentStatus: string) => {
        const newStatus = currentStatus === 'Active' ? 'Maintenance' : 'Active';
        if (!window.confirm(`Змінити статус на ${newStatus}?`)) return;
        try {
            await generationApi.updatePlantStatus(plantId, newStatus);
            fetchPlants();
        } catch (err: any) {
            alert('Помилка: ' + err.response?.data?.error);
        }
    };

    // Відкриває модалку для введення 24 годин
    const openForecastModal = (plantId: number, maxCapacity: number) => {
        setHourlyKw(Array(24).fill(0)); // Скидаємо значення
        setForecastDate(new Date().toISOString().split('T')[0]);
        setForecastModal({ plantId, maxCapacity });
    };

    // Оновлює конкретну годину в масиві
    const handleHourChange = (hourIndex: number, value: string, maxCapacity: number) => {
        let numValue = parseFloat(value) || 0;
        if (numValue > maxCapacity) numValue = maxCapacity; // Валідація максимуму
        if (numValue < 0) numValue = 0;

        const newArr = [...hourlyKw];
        newArr[hourIndex] = numValue;
        setHourlyKw(newArr);
    };

    // Відправляє зібрані 24 значення на сервер
    const submitManualForecast = async () => {
        if (!forecastModal) return;
        
        const hourlyForecasts = hourlyKw.map((kw, index) => ({
            hour: index,
            forecastedKw: kw
        }));

        try {
            await generationApi.submitForecast(forecastModal.plantId, { date: forecastDate, hourlyForecasts });
            alert(`Прогноз на ${forecastDate} успішно подано!`);
            setForecastModal(null); // Закриваємо модалку
        } catch (err: any) {
            alert('Помилка: ' + (err.response?.data?.error || err.message));
        }
    };

    // Відкриває історію з погодинним розкладом
    const handleViewHistory = async (plantId: number) => {

        const start = new Date();
        start.setMonth(start.getMonth() - 1);
        const startStr = start.toISOString().split('T')[0];

        const end = new Date();
        end.setDate(end.getDate() + 14); 
        const endStr = end.toISOString().split('T')[0];

        try {
            // Тепер ми просимо бекенд дати прогнози за ширший період
            const history = await generationApi.getForecastHistory(plantId, startStr, endStr);
            setHistoryModal(history);
        } catch (err: any) {
            alert('Помилка: ' + err.response?.data?.error);
        }
    };

    return (
        <div className="min-h-screen bg-gray-100 p-8 relative">
            <div className="max-w-6xl mx-auto">
                <div className="flex justify-between items-center bg-white p-6 rounded-xl shadow-sm mb-6">
                    <h1 className="text-3xl font-bold text-gray-800">Центр управління ЕС</h1>
                    <div className="space-x-4">
                        {/* <button onClick={() => setShowAddForm(!showAddForm)} className="bg-green-600 text-white px-4 py-2 rounded-md hover:bg-green-700">
                            {showAddForm ? 'Скасувати' : '+ Додати станцію'}
                       </button> */}
                        <button onClick={() => navigate('/profile')} className="bg-gray-100 text-gray-700 px-4 py-2 rounded-md hover:bg-gray-200 border border-gray-300 font-medium">
                            Мій профіль
                        </button>
                        <button onClick={handleLogout} className="bg-red-500 text-white px-4 py-2 rounded-md hover:bg-red-600">
                            Вийти
                        </button>
                    </div>
                </div>

                

                {/* Форма додавання... (Така ж як і була)}
                {showAddForm && (
                    <div className="bg-white p-6 rounded-xl shadow-sm mb-6 border-l-4 border-green-500">
                        <form onSubmit={handleAddPlant} className="flex gap-4 items-end">
                            <div className="flex-1">
                                <label className="block text-sm text-gray-600">Тип</label>
                                <select className="w-full p-2 border rounded-md" value={newPlant.type} onChange={e => setNewPlant({...newPlant, type: e.target.value})}>
                                    <option value="Solar">Сонячна (Solar)</option>
                                    <option value="Wind">Вітрова (Wind)</option>
                                </select>
                            </div>
                            <div className="flex-1">
                                <label className="block text-sm text-gray-600">Потужність (кВт)</label>
                                <input type="number" step="0.1" required className="w-full p-2 border rounded-md" value={newPlant.maxCapacityKw} onChange={e => setNewPlant({...newPlant, maxCapacityKw: parseFloat(e.target.value)})} />
                            </div>
                            <div className="flex-1">
                                <label className="block text-sm text-gray-600">Локація</label>
                                <input type="text" required className="w-full p-2 border rounded-md" value={newPlant.location} onChange={e => setNewPlant({...newPlant, location: e.target.value})} />
                            </div>
                            <button type="submit" className="bg-blue-600 text-white px-6 py-2 rounded-md hover:bg-blue-700">Зберегти</button>
                        </form>
                    </div>
                )*/}

                {/* Список станцій */}
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                    {plants.length === 0 && (
                        <div className="col-span-2 text-center p-10 bg-white rounded-xl shadow-sm text-gray-500">
                            У вас немає електростанцій. Дані з'являться після того, як Оператор мережі внесе їх у систему.
                        </div>
                    )}
                    
                    {plants.map((plant) => (
                        <div key={plant.id} className="bg-white p-6 rounded-xl shadow-sm border border-gray-100 flex flex-col relative">
                            
                            <button 
                                onClick={() => handleStatusChange(plant.id, plant.status)}
                                className="absolute top-4 right-4 text-gray-400 hover:text-gray-700 transition"
                                title={`Перевести станцію в ${plant.status === 'Active' ? 'ремонт' : 'активний стан'}`}
                            >
                                {/* SVG іконка шестірні */}
                                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-6 h-6">
                                  <path strokeLinecap="round" strokeLinejoin="round" d="M9.594 3.94c.09-.542.56-.94 1.11-.94h2.593c.55 0 1.02.398 1.11.94l.213 1.281c.063.374.313.686.645.87.074.04.147.083.22.127.324.196.72.257 1.075.124l1.217-.456a1.125 1.125 0 011.37.49l1.296 2.247a1.125 1.125 0 01-.26 1.431l-1.003.827c-.293.24-.438.613-.431.992a6.759 6.759 0 010 .255c-.007.378.138.75.43.99l1.005.828c.424.35.534.954.26 1.43l-1.298 2.247a1.125 1.125 0 01-1.369.491l-1.217-.456c-.355-.133-.75-.072-1.076.124a6.57 6.57 0 01-.22.128c-.331.183-.581.495-.644.869l-.213 1.28c-.09.543-.56.941-1.11.941h-2.594c-.55 0-1.02-.398-1.11-.94l-.213-1.281c-.062-.374-.312-.686-.644-.87a6.52 6.52 0 01-.22-.127c-.325-.196-.72-.257-1.076-.124l-1.217.456a1.125 1.125 0 01-1.369-.49l-1.297-2.247a1.125 1.125 0 01.26-1.431l1.004-.827c.292-.24.437-.613.43-.992a6.932 6.932 0 010-.255c.007-.378-.138-.75-.43-.99l-1.004-.828a1.125 1.125 0 01-.26-1.43l1.297-2.247a1.125 1.125 0 011.37-.491l1.216.456c.356.133.751.072 1.076-.124.072-.044.146-.087.22-.128.332-.183.582-.495.644-.869l.214-1.281z" />
                                  <path strokeLinecap="round" strokeLinejoin="round" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                                </svg>
                            </button>
                    
                            <div className="flex justify-between items-start mb-4 pr-8">
                                <div>
                                    <h3 className="text-xl font-bold text-gray-800">
                                        {plant.type === 'Solar' ? '☀️ Сонячна ЕС' : '🌬️ Вітрова ЕС'} #{plant.id}
                                    </h3>
                                    <p className="text-gray-500 text-sm mt-1">📍 {plant.location}</p>
                                    
                                    {/* НОВІ ПОЛЯ З DTO */}
                                    <div className="flex gap-2 mt-2">
                                        <span className="bg-blue-50 text-blue-700 text-xs px-2 py-1 rounded border border-blue-100">
                                            Угода #{plant.dealId}
                                        </span>
                                        <span className="bg-purple-50 text-purple-700 text-xs px-2 py-1 rounded border border-purple-100">
                                            Лінія Мережі #{plant.substationLineId}
                                        </span>
                                    </div>
                                </div>
                                
                                <span className={`px-3 py-1 rounded-full text-xs font-bold ${
                                    plant.status === 'Active' ? 'bg-green-100 text-green-700' : 
                                    plant.status === 'Maintenance' ? 'bg-yellow-100 text-yellow-700' : 'bg-red-100 text-red-700'
                                }`}>
                                    {plant.status}
                                </span>
                            </div>
                            
                            <div className="mb-6 mt-2">
                                <p className="text-gray-700 font-medium">Макс: <span className="text-blue-600">{plant.maxCapacityKw} кВт</span></p>
                            </div>
                            
                            <div className="grid grid-cols-2 gap-2 mt-auto">
                                <button 
                                    onClick={() => openForecastModal(plant.id, plant.maxCapacityKw)} 
                                    disabled={plant.status !== 'Active'} 
                                    className="bg-blue-50 text-blue-700 py-2 rounded hover:bg-blue-100 disabled:opacity-50 border border-blue-200 transition"
                                >
                                    Подати прогноз
                                </button>
                                <button 
                                    onClick={() => handleViewHistory(plant.id)} 
                                    className="bg-gray-50 text-gray-700 py-2 rounded hover:bg-gray-100 border border-gray-200 transition"
                                >
                                    Історія
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            </div>

            {/* --- МОДАЛЬНЕ ВІКНО ПОДАЧІ ПРОГНОЗУ --- */}
            {forecastModal && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
                    <div className="bg-white rounded-xl shadow-xl p-6 w-full max-w-3xl max-h-[90vh] overflow-y-auto">
                        <div className="flex justify-between items-center mb-6">
                            <h2 className="text-2xl font-bold">Погодинний прогноз (Макс: {forecastModal.maxCapacity} кВт)</h2>
                            <button onClick={() => setForecastModal(null)} className="text-gray-500 hover:text-red-500 text-2xl font-bold">&times;</button>
                        </div>
                        
                        <div className="mb-6">
                            <label className="block text-gray-700 font-medium mb-2">Оберіть дату:</label>
                            <input type="date" value={forecastDate} onChange={(e) => setForecastDate(e.target.value)} className="w-full md:w-1/3 p-2 border rounded-md" />
                        </div>

                        {/* Сітка на 24 інпути */}
                        <div className="grid grid-cols-4 md:grid-cols-6 gap-4 mb-6">
                            {hourlyKw.map((val, index) => (
                                <div key={index} className="flex flex-col">
                                    <label className="text-xs text-gray-500 mb-1 font-mono">{index.toString().padStart(2, '0')}:00</label>
                                    <input 
                                        type="number" step="0.1" min="0" max={forecastModal.maxCapacity}
                                        value={val || ''}
                                        onChange={(e) => handleHourChange(index, e.target.value, forecastModal.maxCapacity)}
                                        className="p-2 border border-gray-300 rounded focus:border-blue-500 focus:ring-1 focus:ring-blue-500 outline-none"
                                        placeholder="0"
                                    />
                                </div>
                            ))}
                        </div>

                        <div className="flex justify-between items-center bg-gray-50 p-4 rounded-lg">
                            <p className="font-bold text-lg">Загалом: <span className="text-blue-600">{hourlyKw.reduce((a, b) => a + b, 0).toFixed(2)} кВт</span></p>
                            <button onClick={submitManualForecast} className="bg-blue-600 text-white px-6 py-2 rounded-md hover:bg-blue-700 font-medium shadow-md">
                                Відправити прогноз
                            </button>
                        </div>
                    </div>
                </div>
            )}

            {/* --- МОДАЛЬНЕ ВІКНО ІСТОРІЇ --- */}
            {historyModal && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
                    <div className="bg-white rounded-xl shadow-xl p-6 w-full max-w-4xl max-h-[90vh] overflow-y-auto">
                        <div className="flex justify-between items-center mb-4">
                            <h2 className="text-2xl font-bold">Управління прогнозами</h2>
                            <button onClick={() => setHistoryModal(null)} className="text-gray-500 hover:text-red-500 text-2xl font-bold">&times;</button>
                        </div>

                        {/* Вкладки для перемикання */}
                        <div className="flex space-x-6 mb-6 border-b">
                            <button 
                                className={`pb-2 font-medium transition ${historyTab === 'verified' ? 'text-blue-600 border-b-2 border-blue-600' : 'text-gray-500 hover:text-gray-700'}`}
                                onClick={() => setHistoryTab('verified')}
                            >
                                Перевірені (Історія)
                            </button>
                            <button 
                                className={`pb-2 font-medium transition ${historyTab === 'pending' ? 'text-blue-600 border-b-2 border-blue-600' : 'text-gray-500 hover:text-gray-700'}`}
                                onClick={() => setHistoryTab('pending')}
                            >
                                В очікуванні (Нові)
                            </button>
                        </div>
                        
                        {historyModal.filter(day => historyTab === 'verified' ? day.status !== 'Pending' : day.status === 'Pending').length === 0 ? (
                            <p className="text-gray-500 text-center py-8">
                                {historyTab === 'verified' ? 'Немає перевірених прогнозів за цей період.' : 'Немає прогнозів, які очікують на перевірку.'}
                            </p>
                        ) : (
                            <div className="space-y-6">
                                {historyModal
                                    .filter(day => historyTab === 'verified' ? day.status !== 'Pending' : day.status === 'Pending')
                                    .map((day: any) => (
                                    <div key={day.id} className="border border-gray-200 rounded-lg p-4 bg-gray-50">
                                        <div className="flex justify-between items-center mb-4 border-b pb-2">
                                            <h3 className="font-bold text-lg">{day.forecastDate}</h3>
                                            <div className="flex items-center space-x-4">
                                                <span className="text-gray-600 font-mono font-medium">Сума: {day.totalDailyKw} кВт</span>
                                                <span className={`px-2 py-1 rounded text-xs font-bold ${
                                                    day.status === 'Approved' ? 'bg-green-200' : 
                                                    day.status === 'Pending' ? 'bg-yellow-200' : 
                                                    day.status === 'Canceled' ? 'bg-gray-200 text-gray-700' : 'bg-red-200'
                                                }`}>
                                                    {day.status}
                                                </span>
                                                
                                                {/* Кнопка скасування є тільки на вкладці 'pending' */}
                                                {day.status === 'Pending' && (
                                                    <button 
                                                        onClick={async () => {
                                                            if (!window.confirm('Ви впевнені, що хочете скасувати цей прогноз?')) return;
                                                            try {
                                                                await generationApi.cancelForecast(day.id);
                                                                setHistoryModal(null); // Тимчасово закриваємо для оновлення
                                                                alert('Прогноз успішно скасовано!');
                                                            } catch (err: any) {
                                                                alert('Помилка скасування: ' + (err.response?.data?.error || err.message));
                                                            }
                                                        }}
                                                        className="bg-red-500 text-white px-3 py-1 rounded text-sm hover:bg-red-600 transition"
                                                    >
                                                        Скасувати
                                                    </button>
                                                )}
                                            </div>
                                        </div>
                                        
                                        <div className="grid grid-cols-6 md:grid-cols-12 gap-2 text-center text-sm opacity-80">
                                            {day.hourlyForecasts?.map((hourData: any) => (
                                                <div key={hourData.hour} className="bg-white border rounded p-1">
                                                    <div className="text-gray-400 text-xs">{hourData.hour}:00</div>
                                                    <div className={`font-medium ${day.status === 'Canceled' ? 'text-gray-400 line-through' : 'text-blue-600'}`}>
                                                        {hourData.forecastedKw}
                                                    </div>
                                                </div>
                                            ))}
                                        </div>
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>
                </div>
            )}
        </div>
    );
};