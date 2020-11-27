# Video Indexer

1. **Intro**

   Video Indexer jest to usługa stworzona do wyciągania informacji z mediów, szczególnie video. Używa modeli Machine learning które mogą być modyfikowane i rozszerzane. Ato niektóre z informacji które można wyodrębnić:

   - Identyfikacja twarzy
   - Rozpoznawanie tekstu
   - Oznaczanie przedmiotów
   - Segmentacja scen

   Więcej informacji jest wybierane z audio wybranego z pliku wideo, zawierają one transkrypcję słów czy wykrycie emocji z głosu.

   Można używać takowych rezultatów do rozszerzania umiejętności przeszukiwania, ekstrakcji klipów oraz kreowania zdjęć nagłówkowych.

   **Use cases**

   1. Portal zbierający filmy z imprezy i wycinający najciekawsze momenty i automatycznie montujący najciekawsze wycinki.
   2. Portal który automatycznie generuje napisy do filmów. 

2. **How to**

   Należy przejść do [Video Indexer Portal](https://www.videoindexer.ai/), zalogować się i eksportować klucz do subskrypcji, następnie za pomocą kodu lub narzędzia typu Postman. Używając czterech parametrów podanych poniżej 

   ```c#
   var accountId = GetEnvironmentVariable("VIDEO_INDEXER_ACCOUNT"); 
   var location = GetEnvironmentVariable("VIDEO_INDEXER_LOCATION");
   var apiKey = GetEnvironmentVariable("VIDEO_INDEXER_API_KEY"); 
   var apiUrl = "https://api.videoindexer.ai";
   ```

   wysyłamy film jako ciąg bajtów. 

   Następnie na portalu można edytować wszelkie informacje które zostały wykryte przez usługę.

   