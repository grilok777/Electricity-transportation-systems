import axios from 'axios';

// Загальний базовий шлях
const API_URL = 'https://localhost:7100/api';

const $api = axios.create({ baseURL: API_URL });

// Додаємо токен до кожного запиту
$api.interceptors.request.use((config) => {
    const token = localStorage.getItem('operator_token');
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
});

export const gridApi = {
    // ==========================================
    // 1. АВТОРИЗАЦІЯ (OperatorController)
    // ==========================================
    login: async (data: any) => (await $api.post('/Operator/login', data)).data,
    
    // ==========================================
    // 2. ПІДСТАНЦІЇ (SubstationController)
    // ==========================================
    getSubstations: async () => (await $api.get('/Substation/all')).data,
    getSubstationById: async (id: number) => (await $api.get(`/Substation/${id}`)).data,
    createSubstation: async (data: any) => (await $api.post('/Substation', data)).data,
    updateSubstation: async (data: any) => (await $api.put('/Substation', data)).data,
    deleteSubstation: async (id: number) => (await $api.delete(`/Substation/${id}`)).data,
    searchSubstations: async (params: any) => (await $api.get('/Substation/search', { params })).data,

    // ==========================================
    // 3. ЛІНІЇ ПІДСТАНЦІЙ (SubstationLineController) 
    // (Ви їх пропустили, але вони потрібні для підключення станцій!)
    // ==========================================
    getLines: async () => (await $api.get('/SubstationLine/all')).data,
    createLine: async (data: any) => (await $api.post('/SubstationLine', data)).data,
    updateLine: async (data: any) => (await $api.put('/SubstationLine', data)).data,
    deleteLine: async (id: number) => (await $api.delete(`/SubstationLine/${id}`)).data,

    // ==========================================
    // 4. УГОДИ ТА КЛІЄНТИ (OwnerDealController)
    // ==========================================
    getDeals: async (search?: string) => (await $api.get(`/OwnerDeal/search?searchTerm=${search || ''}`)).data,
    
    // ==========================================
    // 5. ЕЛЕКТРОСТАНЦІЇ (PowerPlantController)
    // ==========================================
    getPlants: async (dealId?: number) => (await $api.get(`/PowerPlant/search${dealId ? `?dealId=${dealId}` : ''}`)).data,
    // Оператор призначає лінію підстанції для конкретної електростанції
    updatePlantLine: async (data: any) => (await $api.put('/PowerPlant', data)).data, 

    // ==========================================
    // 6. МОДЕРАЦІЯ ПРОГНОЗІВ (ForecastController / PowerPlantDayController)
    // ==========================================
    getForecasts: async (status?: string) => (await $api.get(`/Forecast/search${status ? `?status=${status}` : ''}`)).data,
    // Зміна статусу прогнозу (Pending -> Approved / Rejected)
    updateForecastStatus: async (id: number, newStatus: string) => (await $api.put('/Forecast', { forecastId: id, newStatus })).data,

    // ==========================================
    // 7. ПЛАНУВАННЯ МЕРЕЖІ (GridDatetimeController / AvailablePower)
    // ==========================================
    getGridDatetimes: async () => (await $api.get('/GridDatetime/all')).data,
    createGridDatetime: async (data: any) => (await $api.post('/GridDatetime', data)).data,
    // Якщо є контролер для AvailablePower:
    createAvailablePower: async (data: any) => (await $api.post('/AvailablePower', data)).data,
};