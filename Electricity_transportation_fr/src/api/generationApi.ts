import { apiClient } from './axiosClient'; // Ваш налаштований axios

export const generationApi = {
    getPlants: async () => {
        const response = await apiClient.get(`/Generation/owner/plants`);
        return response.data;
    },
    registerPlant: async ( data: any) => {
        const response = await apiClient.post(`/Generation/plant`, data);
        return response.data;
    },
    updatePlantStatus: async (plantId: number, status: string) => {
        const response = await apiClient.patch(`/Generation/plant/${plantId}/status?status=${status}`);
        return response.data;
    },
    submitForecast: async (plantId: number, data: any) => {
        const response = await apiClient.post(`/Generation/plant/${plantId}/forecast`, data);
        return response.data;
    },
    getForecastHistory: async (plantId: number, startDate: string, endDate: string) => {
        const response = await apiClient.get(`/Generation/plant/${plantId}/forecast/history?startDate=${startDate}&endDate=${endDate}`);
        return response.data;
    },
    cancelForecast: async (forecastId: number) => {
        const response = await apiClient.patch(`/Generation/forecast/${forecastId}/cancelpending`);
        return response.data;
    }
};