# MovieRestfulAPI
## Arvato .Net Bootcamp Bitirme Projesi


## Tech

- Net 6 Framework
- Ef Core
- Web Api
- Postgresql
- Unit Of Work Design
- Static Factory Method Design
- Dependency Injection
- Autofac
- Swagger
- AutoMApper
- Redis
- Background Job Worker

kullanılarak yapılmıştır.


## Login
Endpointleri kullanabilmeniz için öncelikle giriş yapmalısınız:
![image](https://user-images.githubusercontent.com/50194817/179227903-5f53ef61-4b7b-408d-978c-916fb6d1b282.png)
     `username: admin@mail.com` 
     ` pasword: Password123` 
     
![image](https://user-images.githubusercontent.com/50194817/179230097-4ab2a829-cf94-4961-b047-f8f26a62205f.png) 
Daha sonra dönen yanıttaki Key'i alıp, `Bearer $key` ile giriş yapabilirsiniz.
![image](https://user-images.githubusercontent.com/50194817/179230268-93844b8f-705d-4029-a40c-bfe346e146d4.png)

## Movie API
- Movie List
- Movie Detail
- Create Movie
- Update Movie
- Delete Movie
- Movie List For Genre
- Movie List For Rate
- Movie List For Release Date
- Search (Title, year, rate)

## Genre API
- Genre List
- Update Genre

## Trending API
- List Most Viewed Movies (Top 50)
- List Top Rated Movies (Top 50)

Diğer methodlar (AddRange ve AddRemove) endpoint olarak eklenmemiştir ama methodları Service.cs class'ında mevcut. İncelemek isterseniz oradan bakabilirsiniz. Gerekli durumlarda Service.cs'den çağırılıp eklenebilir.
