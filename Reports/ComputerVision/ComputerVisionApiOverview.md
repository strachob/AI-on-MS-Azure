# Computer Vision API

1. **Intro**

   Computer Vision API jest to zbiór usług, których zadaniem jest pomoc użytkownikowi w przetwarzaniu, zrozumieniu i wydobyciu informacji z obrazu lub szeregu obrazów. Kilkoma przykładami taki usług są na przykład:

   - Face API - które pomaga w rozpoznaniu osoby na zdjęciu, porównaniu dwóch twarz czy wydobyciu konkretnych cech z danej twarzy (od znalezienia czubka nosa do kącików ust czy oczu). Można również grupować zdjęcia należące do jednej osoby czy rodziny.
   - Emotion API - zapewnia zaawansowane algorytmy które ze zdjęcia w różnym poziomie pewności mogą rozpoznać: złość, obrzydzenie, strach, szczęście, zaskoczenie czy nawet brak emocji.
   - Text extraction (OCR) - usługa pomagająca uzyskać tekst z odręcznie pisanych dokumentów których mamy zdjęcia. Działa w wielu językach i rozpoznaje wiele styli pisania. 
   - Image understanding - możemy uruchomić usługę która nam opisze co znajduje się na zdjęciu a tak właściwie co ona myśli, że na nim się znajduje.
   - Spatial analysis - W czasie rzeczywistym możemy analizować ile osób przechodziło w danym miejscu albo która z tras była najczęściej uczęszczana.

2. **Use cases**

   1. W przypadku Image understanding może to być świetne dla osób niewidzących lub słabo widzących. Można by zaprojektować okulary które by używały tego API i w czasie rzeczywistym opisywały człowiekowi co przed sobą ma. 
   2. OCR - w dziekanacie można by użyć tego systemu do skanowania wszelkiego rodzaju dokumentów, które wciąż muszą być wypełniane ręcznie i automatycznie konwertowane byłby do wersji cyfrowej.

3. **How to**

   1. Jak większość usług typu API mamy dostępne enpointy Http do których dostaniemy klucze po utworzeniu zasobu na portalu Azure lub za pomocą szablonu ARM. Wchodzimy wtedy w zakładkę Access Keys, kopiujemy klucz i wykonujmy potrzebne zapytania z poziomu kodu czy przy pomocy narzędzia typu Postman.

   2. Jest też dostępny  [Software Development Kit](https://docs.microsoft.com/pl-pl/azure/cognitive-services/computer-vision/quickstarts-sdk/client-library?pivots=programming-language-csharp&tabs=visual-studio) , który to dostępny jest dostępny obecnie w 5 językach. 