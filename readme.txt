Można odpalić serwisy lokalnie (znacznie lepsze do rozbudowywania aplikacji, bo nie trzeba co chwilę budować obrazów)
1. najpierw można odpalić frontend:
 - npm run start w ./frontend/social-app (powinien odpalić się na porcie 3000)
2. teraz można odpalić backend (IdentityAPI):
taki ConnectionString ma być w appSettings.json lub appSettings.development.json:
"ConnectionStrings": {
  "Connection": "Server=tomi;Database=YourDatabase;Trusted_Connection=True;Encrypt=false"
}, 
backend odpalamy po prostu z Visual Studio, baza powinna się automatycznie stworzyć po odpaleniu serwisu na podstawie migracji

Można też odpalić wszystkie serwisy tak, aby były w kontenerach dockera
0. connectionString w appSettings.json lub appSettings.development.json powinien być postaci:
"ConnectionStrings": {
  "Connection": "Server=sqlserver;Database=YourDatabase;User=sa;Password=YourStrongPassword!"
},
1. przechodzimy do root directory, widzimy, że jest tam plick docker-compose.yml
2. w terminalu wpisujemy komendę: docker compose up --build (odpali się docker compose i zbuduje wszystkie obrazy)
3. powinny stworzyć się 3 kontenery (frontend (port 3000), IdentityAPI(port 5000) oraz baza sqlserver)
4. żeby zobaczyć co znajduje się w środku bazy postawionej w dockerze, możeemy zrobić to poprzez SSMS
    Server name: localhost, 1433
    Authentication: SQL Server Authentication
    Login: sa
    Password: YourStrongPassword!
Login i Password są z wyżej podanego connectionStringa