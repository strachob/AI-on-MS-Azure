# Azure Bot Service

1. **Intro**
   1. Jest to serwis, który pomaga w budowaniu inteligentnych botów do konwersacji z użytkownikiem. Boty takie pomagają użytkownikom np odpowiadając na żmudne pytania FAQ. Zamiast przeszukiwania kopalni pytań i odpowiedzi użytkownik zadanie pytanie, bot rozpoznaje intencje i zwraca odpowiedź na specyficzne pytanie.
   2. Serwis odpowiada za stworzenie bazy wiedzy to znaczy bazy pytań i odpowiedzi. Oraz następnie poprzez odpytanie odpowiednich endpointów zwraca odpowiedź na konkretne pytanie. Serwis jest szeroko rozwijalny i otwarty na połączenia z innymi serwisami mowy i pisma.
2. **Use cases**
   1. Bardzo aktualnym przykładem jest bot sanepidu odpowiedzialny za bazę pytań i odpowiedzi związaną z COVID.
   2. Bot politechniczny do którego podepniemy baze pytań z dziekanatu, pokazywanie pogody i dojazdy na politechnikę. Bardzo przydatny dla studentów pierwszego roku.
3. **How to**
   1. Tworzymy serwis na portalu Azure lub z użyciem ARM Template. Tworzymy bazę wiedzy i publikujemy ją. Po opublikowaniu dostajemy adres API na który musimy wysyłać zapytania. Jednak przy uzyciu Emulatora BOT możemy utrzymywać stałe połączenie.
   2. Pricing - mamy możliwość stworzenia kanału standard albo premium. Obydwie instancje kanału standard mają nielimitowane zapytania ale rózni je szybkość odpowiedzi. Przy kanałach premium i darmowej instancji mamy darmowe 10000 zapytań na miesiac przy płatnej $0.50 za 1000 zapytań.