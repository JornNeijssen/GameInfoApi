# GameInfoAPI Applicatie

## Inhoudsopgave

- [Introductie](#introductie)
- [Benodigdheden](#benodigdheden)
- [Aan de slag](#aan-de-slag)
  - [Clone de repository](#clone-de-repository)
  - [Database configuratie](#database-configuratie)
  - [Applicatie opzetten](#applicatie-opzetten)
- [Gebruik van de API](#gebruik-van-de-api)
- [Tests uitvoeren](#tests-uitvoeren)

## Introductie

De GameInfoAPI is een web api applicatie gebouwd met ASP.NET Core die gamegegevens beheert, waaronder games, auteurs en spelers. Het biedt CRUD-operaties (Create, Read, Update, Delete) via een RESTful API.

## Benodigdheden

Voordat je de applicatie kunt draaien, zorg ervoor dat je het volgende geïnstalleerd hebt:
- .NET 8 SDK
- SQL Server (of een andere ondersteunde database)

## Aan de slag

### Clone de repository

Gebruik Git om de repository lokaal te clonen:

```bash
git clone https://github.com/JornNeijssen/GameInfoApi.git
cd GameInfoAPI
```

### Database configuratie
De applicatie maakt gebruik van een SQL Server-database. Pas de databaseverbinding aan in appsettings.json inclusief jou connection string.

### Applicatie opzetten
Navigeer naar de projectdirectory en voer de volgende stappen uit om de applicatie op te zetten:

```
cd GameInfoAPI
dotnet restore
dotnet build
dotnet run
```

## Gebruik van de API
Je kunt de API endpoints gebruiken met een tool zoals Postman of cURL. Hier zijn enkele voorbeeldendpoints:

GET /api/games: Haal alle spellen op.
POST /api/games: Maak een nieuw spel aan.
GET /api/games/{id}: Haal een specifiek spel op.
PUT /api/games/{id}: Werk een bestaand spel bij.
DELETE /api/games/{id}: Verwijder een spel.

Bij het runnen van de applicatie zal automatisch swagger worden gelaunched hier zijn ook alle endpoints zichtbaar en kan met de database worden gecommuniceerd.

## Tests uitvoeren
De applicatie bevat unit- en integratietests die je kunt uitvoeren om de functionaliteit te verifiëren. Klik met rechtermuisknop op het GameInfoApi.Tests project en klik vervolgens Run tests.



