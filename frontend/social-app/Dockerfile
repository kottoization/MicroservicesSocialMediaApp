# Używamy oficjalnego obrazu Node jako baza
FROM node:18

# Ustawiamy katalog roboczy w kontenerze
WORKDIR /app

# Kopiujemy package.json i package-lock.json, aby zainstalować zależności
COPY package*.json ./

# Instalujemy zależności
RUN npm install

# Kopiujemy całą resztę aplikacji
COPY . .

# Aplikacja będzie działać na porcie 3000
EXPOSE 3000

# Budujemy aplikację i uruchamiamy
CMD ["npm", "start"]