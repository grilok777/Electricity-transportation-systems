import { Outlet, useNavigate, Link, useLocation } from 'react-router-dom';

export const DashboardLayout = () => {
    const navigate = useNavigate();
    const location = useLocation();

    const handleLogout = () => {
        localStorage.removeItem('operator_token');
        navigate('/auth');
    };

    const navItems = [
        { path: '/dashboard/forecasts', label: 'Модерація Прогнозів' },
        { path: '/dashboard/deals', label: 'Клієнти та ЕС' },
        { path: '/dashboard/substations', label: 'Інфраструктура' },
        { path: '/dashboard/planning', label: 'Баланс Мережі' }
    ];

    return (
        <div className="min-h-screen bg-gray-50">
            {/* Header точнісінько як у клієнта */}
            <header className="bg-white shadow-sm px-8 py-4 flex justify-between items-center sticky top-0 z-10">
                <div className="flex items-center gap-12">
                    <h1 className="text-2xl font-bold text-gray-800">
                        Grid<span className="text-blue-600">Control</span>
                    </h1>
                    
                    <nav className="hidden md:flex space-x-2">
                        {navItems.map(item => (
                            <Link 
                                key={item.path} 
                                to={item.path}
                                className={`px-4 py-2 rounded-lg font-medium transition-colors ${
                                    location.pathname === item.path 
                                    ? 'bg-blue-50 text-blue-700' 
                                    : 'text-gray-500 hover:text-gray-800 hover:bg-gray-50'
                                }`}
                            >
                                {item.label}
                            </Link>
                        ))}
                    </nav>
                </div>
                
                <button 
                    onClick={handleLogout} 
                    className="bg-red-50 text-red-600 px-5 py-2 rounded-lg hover:bg-red-100 font-medium transition"
                >
                    Вийти
                </button>
            </header>

            {/* Робоча область */}
            <main className="p-8 max-w-7xl mx-auto">
                <Outlet />
            </main>
        </div>
    );
};