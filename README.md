
<p align="center">
<img src="https://user-images.githubusercontent.com/90132658/176987943-81ee9995-4dd6-4807-907c-69349cb44fe6.png " width="200" height="200" />
</p>


# Project-Omni-Ride-Network [(Car Utility Management)](https://c-u-m.azurewebsites.net/)



Ein Projekt, das im Rahmen der **Webengineering I** [Project-Omni-Ride-Network](https://github.com/Annalytic-programming/Project-Omni-Ride-Network/tree/master/Project-Omni-Ride-Network) und **Projektmanagement** [Projektmanagement](https://github.com/Annalytic-programming/Project-Omni-Ride-Network/tree/master/Projektmanagement) Vorlesungen 
der Dualen Hochschule Baden-Württemberg Karlsruhe [
karlsruhe.dhbw.de/](karlsruhe.dhbw.de/) entstanden ist.

Car Utility Management ist ein fiktiver Carsharing Anbieter, den wir für dieses Projekt erfunden haben. 

Die Funktionen der Webseite beinhalten das erstellen eines Profils, um Fahrzeuge von verschiedenen Anbietern buchen zu können, sowie eine Bewertung der Seite zu hinterlassen. Natürlich sind die Buchungen rein fiktiv, es wird dabei kein Geld abgebucht oder Fahrzeug verbucht.

Viel Spaß und gute Fahrt :red_car: :motorcycle:

## Dokumentation 
Die technische Dokumentation gibt es [hier](https://github.com/Annalytic-programming/Project-Omni-Ride-Network/blob/master/Projektmanagement/Dokumentation.pdf)

## Installation

Um das Projekt lokal aufzusetzen:

``` # Projekt klonen
 
git clone https://github.com/Annalytic-programming/Project-Omni-Ride-Network.git

```

## Konfigurationsdatei anlegen

Füge eine 'appsettings.json' Datei im Verzeichnis [Project-Omni-Ride-Network/Project-Omni-Ride-Network/](https://github.com/Annalytic-programming/Project-Omni-Ride-Network/tree/master/Project-Omni-Ride-Network), nach diesem Muster, ein: 

``` {
    "MailCredentials": {
        "Hostname": "HOSTNAME",
        "Port": PORT,
        "Email": "EMAIL",
        "Passwort": "PASSWORD"
    },

    "ConnectionStrings": {
        "DefaultConnection": "CONNECTION STRING FOR DATABASE"
    },
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information"
        }
    },
    "JWT": {
        "Audience": "AUDIENCE",
        "Issuer": "ISSUER",
        "Secret": "SECRET"
    },
    "AllowedHosts": "*"

}
``` 
<sub>* Wechsle in CAPS geschriebene Wörter mit deinen eigenen Daten aus</sub>



## Unser Team

**Frontend-Developers:** : [@jas20202](https://github.com/jas20202) - [@elnaz-gharoon](https://github.com/elnaz-gharoon)

**Backend-Developers:** [@Annalytic-programming](https://github.com/Annalytic-programming) - [@AlexMi-Ha](https://github.com/AlexMi-Ha)

