# SimpleChatApi

Проект представляет собой ASP.NET Core Web API для управления чатами. Общение в чатах реализовано с помощью SignalR
<h4>В проекте реализовано следующее:</h4> 
<ol>
  <li>Методы API</li>
  <li>SignalR Hub</li>
  <li>Документация API с использованием Swagger.</li>
  
</ol>
<summary><h4>Методы API:</h4></summary>
<ol>
  <li><b>Создание чата:</b>
      <ul>
        <li>HTTP метод: Post</li>
        <li>Путь: /api/Chat/CreateChat</li>
        <li>Описание: Метод позволяет создать чат.</li>
      </ul>
  </li>
  <li><b>Получение чата по названию:</b>
      <ul>
        <li>HTTP метод: GET</li>
        <li>Путь: /api/Chat/GetChat</li>
        <li>Описание: Метод позволяет получить информацию о чате по его названию.</li>
      </ul>
  </li>
  <li><b>Удаление чата:</b>
      <ul>
        <li>HTTP метод:  POST</li>
        <li>Путь: /api/Chat/DeleteChat</li>
        <li>Описание: Метод позволяет удалить чат.</li>
      </ul>
  </li>
</ol>

<summary><h4>SignalR Hub:</h4></summary>
  URL для установки соединения с хабом: <b>https://localhost:port/chat </b>, где "port" - это порт, на котором запущен проект.
<ol>
  <h4>Методы SignalR Hub:</h4>
  <li><b>Send</b> - отправить сообщения в группу</li>
  <li><b>Join</b> - присоединиться к группе</li>
  <li><b>Left</b> - покинуть группу</li>
</ol>

**Доступ к документации:**
<ol>
  <li>Запустите API</li>
  <li>Перейдите по адресу https://localhost:port/swagger/index.html в браузере, где "port" - это порт, на котором запущен проект.</li>
</ol>

<b>Инструкции по установке и использованию:</b>
<ol>
  <li>Клонируйте репозиторий.</li>
  <li>Перейдите в каталог проекта.</li>
  <li>Убедитесь, что у вас установлен .NET SDK. Если нет, установите его с официального сайта .NET: https://dotnet.microsoft.com/download/dotnet</li>
  <li>Восстановите зависимости проекта с помощью команды: dotnet restore</li>
  <li>Запустите API с помощью команды:  dotnet run</li>  
</ol>
