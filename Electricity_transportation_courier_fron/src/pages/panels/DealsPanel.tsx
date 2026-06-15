import { useState, useEffect } from 'react';
import { gridApi } from '../../api/gridApi';

export const DealsPanel = () => {
    const [deals, setDeals] = useState<any[]>([]);
    const [plants, setPlants] = useState<any[]>([]);
    const [loading, setLoading] = useState(true);

    const fetchData = async () => {
        setLoading(true);
        try {
            setDeals(await gridApi.getDeals());
            setPlants(await gridApi.getPlants());
        } catch (err) { console.error(err); } 
        finally { setLoading(false); }
    };

    useEffect(() => { fetchData(); }, []);

    const handleAssignLine = async (plantId: number, currentLineId: number | null) => {
        const newLineStr = window.prompt("Введіть ID лінії підстанції для підключення:", currentLineId?.toString() || "");
        if (!newLineStr) return;
        
        try {
            await gridApi.updatePlantLine({ id: plantId, substationLineId: parseInt(newLineStr), status: 'Active' });
            fetchData();
        } catch (err) { alert("Помилка оновлення станції."); }
    };

    if (loading) return <div className="text-gray-500">Завантаження даних...</div>;

    return (
        <div>
            <h2 className="text-2xl font-bold text-gray-800 mb-6">Клієнти та Електростанції</h2>
            
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
                {/* Картка 1: Угоди */}
                <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-100">
                    <h3 className="text-lg font-bold text-gray-800 mb-4 pb-3 border-b border-gray-100">Діючі Угоди</h3>
                    <div className="space-y-3">
                        {deals.map(deal => (
                            <div key={deal.id} className="p-4 bg-gray-50 rounded-lg border border-gray-100">
                                <div className="flex justify-between items-center mb-2">
                                    <span className="font-bold text-gray-800">{deal.ownerName}</span>
                                    <span className="text-gray-400 text-sm font-mono">#{deal.id}</span>
                                </div>
                                <div className="text-sm text-gray-500 flex flex-col gap-1">
                                    <span>📞 {deal.numberPhone}</span>
                                    <span>📍 {deal.placeLocation}</span>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>

                {/* Картка 2: Станції */}
                <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-100">
                    <h3 className="text-lg font-bold text-gray-800 mb-4 pb-3 border-b border-gray-100">Фізичні Активи (ЕС)</h3>
                    <div className="space-y-3">
                        {plants.map(plant => (
                            <div key={plant.id} className={`p-4 rounded-lg border ${plant.substationLineId ? 'border-gray-100 bg-white' : 'border-red-100 bg-red-50'}`}>
                                <div className="flex justify-between items-start">
                                    <div>
                                        <div className="font-bold text-gray-800 flex items-center gap-2">
                                            {plant.type} ЕС 
                                            <span className="text-xs font-normal text-gray-400">ID: {plant.id}</span>
                                        </div>
                                        <p className="text-sm text-gray-500 mt-1">Потужність: {plant.maxCapacityKw} кВт</p>
                                        <p className="text-xs text-gray-400 mt-1">📍 {plant.location}</p>
                                    </div>
                                    
                                    <div className="text-right">
                                        {plant.substationLineId ? (
                                            <div className="flex flex-col items-end">
                                                <span className="inline-flex items-center gap-1 px-2.5 py-1 bg-green-50 text-green-700 text-xs rounded-md border border-green-100 font-medium">
                                                    🟢 Лінія #{plant.substationLineId}
                                                </span>
                                                <button onClick={() => handleAssignLine(plant.id, plant.substationLineId)} className="text-xs text-blue-500 hover:text-blue-700 mt-2 font-medium">
                                                    Змінити лінію
                                                </button>
                                            </div>
                                        ) : (
                                            <button 
                                                onClick={() => handleAssignLine(plant.id, null)} 
                                                className="bg-red-500 text-white text-xs px-4 py-2 rounded-md hover:bg-red-600 transition shadow-sm"
                                            >
                                                Призначити лінію
                                            </button>
                                        )}
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    );
};