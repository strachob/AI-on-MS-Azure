# Mov4Anyone

### 1. Wstęp

Mov4Anyone jest botem przygotowanym dla ludzi, którzy chcą sprawdzić informacje na temat filmów, seriali czy osób pracujących w biznesie kinowym.

Bot działa najlepiej przeglądając informacje o Hollywood aczkolwiek polskie nazwiska też da się tam znaleźć.

Bot został stworzony z użyciem Microsoft Bot Framework oraz przykładowego kodu [Link](https://github.com/microsoft/BotBuilder-Samples/tree/main/samples/csharp_dotnetcore/13.core-bot) z repozytorium Microsoftu.

Bot używa trzech zewnętrznych serwisów:

1. TMDB API - api serwisu z bazą danych o filmach i biznesie.

2. LUIS - rozpoznawanie intencji użytkownika wraz z encjami w celu znalezienia potrzebnych informacji 

3. Speech to text - serwis potrzebny do transkrypcji mowy na zapytania do bota. Brak konieczności wpisywania wszystkiego z klawiatury.

   #### Demo

   Link do dema bota w serwisie Youtube: [Youtube](https://youtu.be/wfi6MjV4kuA)

### 2. Wprowadzanie zapytań za pomocą głosu

W każdym momencie bot daje nam możliwość wprowadzenia naszego zapytania za pomocą głosu. 

Można to zrobić przy użyciu przycisku "Tap to speek". Wtedy bot oczekuje na wprowadzenie przez nas głosu.

Po 3 sekundach od końca zdania zapytanie nasza mowa jest transkrybowana na tekst który dalej używany jest w dialogu.



### 3. Przykładowy przypadek użycia

1. Uruchamiany bota

2. Pytamy "Is there a movie called The lord of the rings"?

3. Bot wyświetla 5 propozycji filmów o które mogło nam chodzić

4. Wybieramy pierwszą opcję od góry

5. Bot wyświetla nam detale tego filmu (The lord of the rings: The fellowship of the rings)

6. Wpisujemy "I would like to see trailer for The lord of the rings movie"

7. Bot wyświetla nam zwiastun z serwisu youtube

   

### 4. Jak projekt został stworzony?

1. Stworzenie projektu w .NET Core 3.1 z przykładu podanego w punkcie 1.
2. W portalu Azure utworzenie zasobu LUIS
3. Dodanie do LUIS potrzebnych intencji oraz encji -> wytrenowanie modelu oraz wystawienie endpointów (Publish production)
4. Integracja LUISa z projektem w .Net Core [Link](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-howto-v4-luis?view=azure-bot-service-4.0&tabs=csharp#configure-your-bot-to-use-your-luis-app)
5. Wygenerowanie klucza do TMDB oraz integracja potrzbenych zapytań i modeli do serwisu w .NET Core [Link](https://developers.themoviedb.org/3/getting-started/introduction)
6. Utworzenie okienek które bot zwraca z informacjami z użyciem Adaptive Cards
7. Utworzenie zasobu Speech w portalu Azure oraz integracja z .NET Core [Link](https://docs.microsoft.com/en-us/learn/modules/transcribe-speech-input-text/7-optional-exercise-listen-incoming-data)



### 5.  Jak go postawić u siebie?

1. Zainstalować środowisko .Net Core w wersji 3.1

2. Zainstalować Bot Framework Emulator w najnowszej wersji

3. Pobrać repozytorium z kodem

4. Wystawiamy usługę Luis z użyciem pliku .**json** znajdującego się w folderze *CognitiveModels* 

5. Otworzyć plik appsettings.json i zmienić dane zamieszczone poniżej

   ```json
   {
     "TMDB_Key": "<Klucz wygenerowany na TMDB>",
     "LuisAppId": "<Klucz z LUISa - AppId>",
     "LuisAPIKey": "<Klucz z LUISa z zakładki Authoring>",
     "LuisAPIHostName": "<your_region>.api.cognitive.microsoft.com",
     "Speech_Key": "<Klucz do usługi Speech z Portalu Azure>"
   }
   ```

6. Uruchomić lokalnie 

7. Włączyć Emulator i połączyć się z http://localhost:3978/api/messages





