import { useState } from 'react';
import { userApi } from '../api/userApi';

export const AuthPage = () => {
    // ЗМІНЕНО: Замість isLogin використовуємо mode, який може мати 3 стани
    const [mode, setMode] = useState<'login' | 'register' | 'reset'>('login');
    
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [resetCode, setResetCode] = useState(''); // НОВИЙ СТАН: Код відновлення
    const [showPassword, setShowPassword] = useState(false);
    const [error, setError] = useState('');

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');

        try {
            if (mode === 'login') {
                // Авторизація
                const result = await userApi.login({ username, password });
                const token = result.token || result.Token;

                if (!token) throw new Error("Сервер не повернув токен.");

                localStorage.setItem('token', token); 
                window.location.href = '/dashboard'; 
                
            } else if (mode === 'register') {
                // Реєстрація
                await userApi.register({ username, password });
                alert('Успішна реєстрація! Тепер увійдіть.');
                setMode('login');
                setPassword(''); 
                
            } else if (mode === 'reset') {
                // НОВЕ: Відновлення пароля
                await userApi.resetPassword({ 
                    username: username, 
                    resetCode: resetCode, 
                    newPassword: password 
                });
                alert('Пароль успішно відновлено! Тепер увійдіть з новим паролем.');
                setMode('login');
                setPassword('');
                setResetCode('');
            }
        } catch (err: any) {
            console.error("Помилка:", err);
            setError(err.response?.data?.error || err.message || 'Сталася помилка при з\'єднанні з сервером');
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-50">
            <div className="bg-white p-8 rounded-xl shadow-lg max-w-sm w-full">
                <h2 className="text-2xl font-bold text-center text-gray-800 mb-6">
                    {mode === 'login' && 'Вхід в систему'}
                    {mode === 'register' && 'Реєстрація'}
                    {mode === 'reset' && 'Відновлення пароля'}
                </h2>
                
                {error && <div className="bg-red-100 text-red-600 p-3 rounded mb-4 text-sm">{error}</div>}

                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Ім'я користувача</label>
                        <input 
                            type="text" 
                            className="mt-1 w-full p-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 outline-none"
                            value={username}
                            onChange={e => setUsername(e.target.value)}
                            required
                        />
                    </div>

                    {/* Показуємо поле для коду тільки в режимі відновлення */}
                    {mode === 'reset' && (
                        <div>
                            <label className="block text-sm font-medium text-gray-700">16-значний код відновлення</label>
                            <input 
                                type="text" 
                                placeholder="A1B2C3D4E5F6G7H8"
                                className="mt-1 w-full p-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 outline-none font-mono"
                                value={resetCode}
                                onChange={e => setResetCode(e.target.value)}
                                required
                            />
                        </div>
                    )}

                    <div>
                        <label className="block text-sm font-medium text-gray-700">
                            {mode === 'reset' ? 'Новий пароль' : 'Пароль'}
                        </label>
                        <div className="relative mt-1">
                            <input 
                                type={showPassword ? "text" : "password"} 
                                className="w-full p-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 outline-none pr-10"
                                value={password}
                                onChange={e => setPassword(e.target.value)}
                                required
                            />
                            <button
                                type="button"
                                className="absolute inset-y-0 right-0 pr-3 flex items-center text-gray-400 hover:text-gray-600"
                                onClick={() => setShowPassword(!showPassword)}
                            >
                                {showPassword ? (
                                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-5 h-5"><path strokeLinecap="round" strokeLinejoin="round" d="M3.98 8.223A10.477 10.477 0 001.934 12C3.226 16.338 7.244 19.5 12 19.5c.993 0 1.953-.138 2.863-.395M6.228 6.228A10.45 10.45 0 0112 4.5c4.756 0 8.773 3.162 10.065 7.498a10.523 10.523 0 01-4.293 5.774M6.228 6.228L3 3m3.228 3.228l3.65 3.65m7.894 7.894L21 21m-3.228-3.228l-3.65-3.65m0 0a3 3 0 10-4.243-4.243m4.242 4.242L9.88 9.88" /></svg>
                                ) : (
                                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-5 h-5"><path strokeLinecap="round" strokeLinejoin="round" d="M2.036 12.322a1.012 1.012 0 010-.639C3.423 7.51 7.36 4.5 12 4.5c4.638 0 8.573 3.007 9.963 7.178.07.207.07.431 0 .639C20.577 16.49 16.64 19.5 12 19.5c-4.638 0-8.573-3.007-9.963-7.178z" /><path strokeLinecap="round" strokeLinejoin="round" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" /></svg>
                                )}
                            </button>
                        </div>
                        {/* Кнопка "Забули пароль" (тільки при логіні) */}
                        {mode === 'login' && (
                            <div className="text-right mt-1">
                                <button 
                                    type="button" 
                                    onClick={() => { setMode('reset'); setError(''); }} 
                                    className="text-xs text-blue-600 hover:text-blue-800 transition"
                                >
                                    Забули пароль?
                                </button>
                            </div>
                        )}
                    </div>

                    <button 
                        type="submit" 
                        className="w-full bg-blue-600 text-white font-bold py-2 px-4 rounded-md hover:bg-blue-700 transition duration-200 mt-2"
                    >
                        {mode === 'login' && 'Увійти'}
                        {mode === 'register' && 'Зареєструватися'}
                        {mode === 'reset' && 'Відновити пароль'}
                    </button>
                </form>

                <div className="mt-6 text-center text-sm text-gray-600 flex flex-col space-y-2">
                    {mode !== 'login' && (
                        <button onClick={() => { setMode('login'); setError(''); }} className="text-blue-600 font-bold hover:text-blue-800 transition">
                            ← Повернутися до входу
                        </button>
                    )}
                    
                    {mode === 'login' && (
                        <p>
                            Немає акаунту?{' '}
                            <button onClick={() => { setMode('register'); setError(''); }} className="text-blue-600 font-bold hover:text-blue-800 transition">
                                Створити
                            </button>
                        </p>
                    )}
                </div>
            </div>
        </div>
    );
};