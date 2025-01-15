import { useEffect, useState } from 'react';
import './App.css';

interface Forecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

function App() {
    const [forecasts, setForecasts] = useState<Forecast[]>();

    useEffect(() => {
        populateWeatherData();
    }, []);

    const contents = forecasts === undefined
        ? <p><em>Загрузка... Пожалуйста, обновите страницу, как только сервер ASP.NET будет готов. Дополнительные инструкции можно найти по ссылке: <a href="https://aka.ms/jspsintegrationreact" target="_blank" rel="noreferrer">https://aka.ms/jspsintegrationreact</a></em></p>
        : <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Дата</th>
                    <th>Температура (C)</th>
                    <th>Температура (F)</th>
                    <th>Описание</th>
                </tr>
            </thead>
            <tbody>
                {forecasts.map(forecast =>
                    <tr key={forecast.date}>
                        <td>{forecast.date}</td>
                        <td>{forecast.temperatureC}</td>
                        <td>{forecast.temperatureF}</td>
                        <td>{forecast.summary}</td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            <h1>Привет, команда разработчиков!</h1>
            <p>Приложение успешно задеплоено на сервер и работает корректно.</p>
            <div className="info-box">
                <p>Тестирование деплоя: сервер работает, данные о погоде загружены успешно.</p>
                <p>При мердже или пуше в ветку main обновления будут автоматически деплоиться сюда.</p>
            </div>
            <h2>Weather forecast</h2>
            {contents}
            <div className="footer">
                <p>&copy; 2025 TavernHelios. Все права защищены.</p>
            </div>
        </div>
    );

    async function populateWeatherData() {
        const response = await fetch('weatherforecast');
        if (response.ok) {
            const data = await response.json();
            setForecasts(data);
        }
    }
}

export default App;
