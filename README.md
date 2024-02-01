[Build Api Image]
- cd C:\Users\costa\source\repos\xTheCryptoKeeperx
- docker build -t tck-api .

[Build Client Image]
- cd TCK.Bot.UI
- docker build -t tck-client .

[Start both images]
docker-compose -f tck-docker-compose.yml up