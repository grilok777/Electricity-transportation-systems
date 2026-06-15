import { useState, useEffect } from 'react';
import { gridApi } from '../../api/gridApi';

export const ForecastsPanel = () => {
    const [forecasts, setForecasts] = useState<any[]>([]);
    const [loading, setLoading] = useState(true);
    const [filter, setFilter] = useState('Pending');

    const fetchForecasts = async () => {
        setLoading(true);
        try {
            const data = await gridApi.getForecasts(filter !== 'All' ? filter : undefined);
            setForecasts(data);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => { fetchForecasts(); }, [filter]);

    const handleStatusUpdate = async (id: number, newStatus: string) => {
        if (!window.confirm(`Змінити статус на ${newStatus}?`)) return;
        try {
            await gridApi.updateForecastStatus(id, newStatus);
            fetchForecasts();
        } catch (err) {
            alert("Помилка оновлення статусу");
        }
    };

    return (
        <div>
            <h2 className="text-2xl font-bold text-gray-800 mb-6">Модерація Прогнозів</h2>
            
            {/* Вкладки (Tabs) в стилі Generation */}
            <div className="flex space-x-8 border-b border-gray-200 mb-8 overflow-x-auto">
                {['Pending', 'Approved', 'Rejected', 'All'].map(status => (
                    <button 
                        key={status}
                        onClick={() => setFilter(status)}
                        className={`pb-3 font-medium transition-colors whitespace-nowrap ${
                            filter === status 
                            ? 'border-b-2 border-blue-600 text-blue-600' 
                            : 'text-gray-500 hover:text-gray-800'
                        }`}
                    >
                        {status === 'Pending' ? '🔴 Очікують рішення' : 
                         status === 'Approved' ? '🟢 Затверджені' : 
                         status === 'Rejected' ? '⚫ Відхилені' : 'Всі прогнози'}
                    </button>
                ))}
            </div>

            {loading ? <div className="text-gray-500">Завантаження даних...</div> : (
                <div className="space-y-6">
                    {forecasts.length === 0 && (
                        <div className="bg-white p-8 rounded-xl shadow-sm border border-gray-100 text-center text-gray-500">
                            Прогнозів у цій категорії не знайдено.
                        </div>
                    )}
                    
                    {forecasts.map(day => (
                        <div key={day.id} className="bg-white p-6 rounded-xl shadow-sm border border-gray-100">
                            <div className="flex justify-between items-start border-b border-gray-100 pb-4 mb-4">
                                <div>
                                    <h3 className="text-lg font-bold text-gray-800">Станція #{day.plantId} <span className="text-gray-400 font-normal">| {day.forecastDate}</span></h3>
                                    <p className="text-gray-500 mt-1">Загальна генерація: <span className="font-bold text-gray-700">{day.totalDailyKw} кВт</span></p>
                                </div>
                                <div className="flex items-center gap-4">
                                    <span className={`px-3 py-1 rounded-full text-xs font-bold ${
                                        day.status === 'Pending' ? 'bg-yellow-100 text-yellow-700' :
                                        day.status === 'Approved' ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'
                                    }`}>
                                        {day.status}
                                    </span>
                                    
                                    {day.status === 'Pending' && (
                                        <div className="flex gap-2 ml-4">
                                            <button onClick={() => handleStatusUpdate(day.id, 'Approved')} className="bg-green-500 hover:bg-green-600 text-white px-4 py-1.5 rounded-md text-sm font-medium transition">
                                                Затвердити
                                            </button>
                                            <button onClick={() => handleStatusUpdate(day.id, 'Rejected')} className="bg-red-50 text-red-600 hover:bg-red-100 border border-red-200 px-4 py-1.5 rounded-md text-sm font-medium transition">
                                                Відхилити
                                            </button>
                                        </div>
                                    )}
                                </div>
                            </div>

                            <div className="grid grid-cols-6 md:grid-cols-12 gap-3 text-center">
                                {day.hourlyForecasts?.map((hour: any) => (
                                    <div key={hour.hour} className="bg-gray-50 p-2 rounded-lg border border-gray-100">
                                        <div className="text-gray-400 text-xs mb-1">{hour.hour}:00</div>
                                        <div className="font-bold text-gray-700 text-sm">{hour.forecastedKw}</div>
                                    </div>
                                ))}
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};