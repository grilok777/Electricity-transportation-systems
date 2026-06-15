import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthPage } from './pages/AuthPage';
import { DashboardLayout } from './pages/DashboardLayout';
import { SubstationsPanel } from './pages/panels/SubstationsPanel';
import { ForecastsPanel } from './pages/panels/ForecastsPanel';
import { DealsPanel } from './pages/panels/DealsPanel';

function App() {
  const isAuth = !!localStorage.getItem('operator_token');

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/auth" element={!isAuth ? <AuthPage /> : <Navigate to="/dashboard/forecasts" />} />
        
        {/* Головний Лейаут Адмінки */}
        <Route path="/dashboard" element={isAuth ? <DashboardLayout /> : <Navigate to="/auth" />}>
            <Route path="forecasts" element={<ForecastsPanel />} />
            <Route path="substations" element={<SubstationsPanel />} />
            <Route path="deals" element={<DealsPanel />} />
            <Route index element={<Navigate to="forecasts" />} />
        </Route>

        <Route path="*" element={<Navigate to="/dashboard/forecasts" />} />
      </Routes>
    </BrowserRouter>
  );
}
export default App;