import { useState, useEffect } from 'react';
import { gridApi } from '../../api/gridApi';

export const SubstationsPanel = () => {
    const [substations, setSubstations] = useState<any[]>([]);

    useEffect(() => {
        const fetchSubs = async () => {
            try {
                const data = await gridApi.getSubstations();
                setSubstations(data);
            } catch (err) {
                console.error("Помилка завантаження підстанцій");
            }
        };
        fetchSubs();
    }, []);

    return (
        <div>
            <div className="flex justify-between items-center mb-6 bg-white p-6 rounded-xl shadow-sm border border-gray-100">
                <h2 className="text-2xl font-bold text-gray-800">Інфраструктура Мережі</h2>
                <button className="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 font-medium transition">
                    + Нова Підстанція
                </button>
            </div>
            
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {substations.map(sub => (
                    <div key={sub.id} className="bg-white rounded-xl shadow-sm border border-gray-100 border-t-4 border-t-blue-500 overflow-hidden relative">
                        <div className="p-6">
                            <div className="flex justify-between items-start mb-2">
                                <h3 className="font-bold text-xl text-gray-800">{sub.name}</h3>
                                <span className={`px-3 py-1 rounded-full text-xs font-bold ${
                                    sub.status === 'Active' ? 'bg-green-100 text-green-700' : 
                                    sub.status === 'Maintenance' ? 'bg-yellow-100 text-yellow-700' : 
                                    'bg-red-100 text-red-700'
                                }`}>
                                    {sub.status}
                                </span>
                            </div>
                            
                            <p className="text-gray-500 text-sm mb-6">📍 {sub.location}</p>
                            
                            <div className="bg-gray-50 p-4 rounded-lg border border-gray-100">
                                <h4 className="font-bold text-sm text-gray-700 mb-3 border-b pb-2">Підключені Лінії</h4>
                                <ul className="space-y-2">
                                    {sub.substationLines?.map((line: any) => (
                                        <li key={line.id} className="flex justify-between text-sm items-center">
                                            <span className="font-medium text-gray-600">Лінія #{line.id}</span>
                                            <span className="font-mono text-blue-600 bg-blue-50 px-2 py-1 rounded">{line.baseLoadKw} кВт</span>
                                        </li>
                                    ))}
                                    {!sub.substationLines?.length && <p className="text-xs text-gray-400 italic">Ліній немає</p>}
                                </ul>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};