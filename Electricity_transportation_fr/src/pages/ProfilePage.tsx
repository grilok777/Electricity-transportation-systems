import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { userApi } from '../api/userApi';

export const ProfilePage = () => {
    const navigate = useNavigate();

    const [profile, setProfile] = useState<any>(null);
    const [loading, setLoading] = useState(true);

    // Стан для форми зміни пароля
    const [oldPassword, setOldPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');

    // Стан для форми контактів угоди
    const [dealId, setDealId] = useState('');
    const [numberPhone, setNumberPhone] = useState('');
    const [placeLocation, setPlaceLocation] = useState('');

    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const data = await userApi.getProfile();
                setProfile(data);
            } catch (err) {
                alert("Помилка завантаження профілю");
            } finally {
                setLoading(false);
            }
        };
        fetchProfile();
    }, []);

    const handlePasswordChange = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await userApi.changePassword({ oldPassword, newPassword });
            alert('Пароль успішно змінено!');
            setOldPassword('');
            setNewPassword('');
        } catch (err: any) {
            alert('Помилка: ' + (err.response?.data?.error || err.message));
        }
    };

    const handleContactUpdate = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!dealId) return alert("Введіть номер угоди!");
        
        try {
            await userApi.updateContactInfo(Number(dealId), { numberPhone, placeLocation });
            alert(`Контакти для угоди №${dealId} оновлено!`);
            setDealId('');
            setNumberPhone('');
            setPlaceLocation('');
        } catch (err: any) {
            alert('Помилка: ' + (err.response?.data?.error || err.message));
        }
    };

    if (loading) return <div className="p-10 text-center">Завантаження...</div>;

    return (
        <div className="min-h-screen bg-gray-100 p-8">
            <div className="max-w-4xl mx-auto space-y-6">
                
                {/* Хедер */}
                <div className="flex justify-between items-center bg-white p-6 rounded-xl shadow-sm">
                    <h1 className="text-3xl font-bold text-gray-800">Налаштування профілю</h1>
                    <button 
                        onClick={() => navigate('/dashboard')}
                        className="text-blue-600 hover:text-blue-800 font-medium"
                    >
                        ← Повернутися до станцій
                    </button>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                    {/* Секція 1: Інформація */}
                    <div className="bg-white p-6 rounded-xl shadow-sm border-t-4 border-blue-500">
                        <h2 className="text-xl font-bold mb-4">Особисті дані</h2>
                        <div className="space-y-3 text-gray-700">
                            <p><span className="font-semibold">ID Користувача:</span> {profile?.id}</p>
                            <p><span className="font-semibold">Логін:</span> {profile?.username}</p>
                            <p><span className="font-semibold">Дата реєстрації:</span> {profile?.dateRegistration}</p>
                        </div>
                    </div>

                    {/* Секція 2: Зміна пароля */}
                    <div className="bg-white p-6 rounded-xl shadow-sm border-t-4 border-yellow-500">
                        <h2 className="text-xl font-bold mb-4">Зміна пароля</h2>
                        <form onSubmit={handlePasswordChange} className="space-y-4">
                            <div>
                                <label className="block text-sm text-gray-600">Старий пароль</label>
                                <input 
                                    type="password" required
                                    className="w-full p-2 border rounded-md mt-1"
                                    value={oldPassword} onChange={e => setOldPassword(e.target.value)}
                                />
                            </div>
                            <div>
                                <label className="block text-sm text-gray-600">Новий пароль</label>
                                <input 
                                    type="password" required
                                    className="w-full p-2 border rounded-md mt-1"
                                    value={newPassword} onChange={e => setNewPassword(e.target.value)}
                                    placeholder="Мінімум 1 цифра і 1 велика літера"
                                />
                            </div>
                            <button type="submit" className="w-full bg-yellow-500 text-white font-bold py-2 rounded-md hover:bg-yellow-600">
                                Оновити пароль
                            </button>
                        </form>
                    </div>

                    {/* Секція 3: Оновлення контактів угоди */}
                    <div className="bg-white p-6 rounded-xl shadow-sm border-t-4 border-green-500 md:col-span-2">
                        <h2 className="text-xl font-bold mb-2">Контакти за угодами</h2>
                        <p className="text-sm text-gray-500 mb-4">Оскільки ваші контакти прив'язані до конкретних угод, вкажіть ID угоди для оновлення.</p>
                        
                        <form onSubmit={handleContactUpdate} className="flex flex-col md:flex-row gap-4 items-end">
                            <div className="w-full md:w-1/4">
                                <label className="block text-sm text-gray-600">ID Угоди</label>
                                <input 
                                    type="number" required placeholder="Напр. 12"
                                    className="w-full p-2 border rounded-md mt-1"
                                    value={dealId} onChange={e => setDealId(e.target.value)}
                                />
                            </div>
                            <div className="w-full md:w-2/4">
                                <label className="block text-sm text-gray-600">Номер телефону</label>
                                <input 
                                    type="text" required placeholder="+380..."
                                    className="w-full p-2 border rounded-md mt-1"
                                    value={numberPhone} onChange={e => setNumberPhone(e.target.value)}
                                />
                            </div>
                            <div className="w-full md:w-2/4">
                                <label className="block text-sm text-gray-600">Локація (Адреса)</label>
                                <input 
                                    type="text" required
                                    className="w-full p-2 border rounded-md mt-1"
                                    value={placeLocation} onChange={e => setPlaceLocation(e.target.value)}
                                />
                            </div>
                            <button type="submit" className="w-full md:w-auto bg-green-600 text-white font-bold px-6 py-2 rounded-md hover:bg-green-700">
                                Зберегти
                            </button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
};