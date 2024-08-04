public static class SeedData
{
    public static void EnsurePopulated(IApplicationBuilder app)
    {
        PizzaDb context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<PizzaDb>();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product
                {
                    Name = "Омлет с ветчиной и грибами",
                    ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE7970321044479C1D1085457A36EB.webp",
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Омлет с пепперони",
                    ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE94ECF33B0C46BA410DEC1B1DD6F8.webp",
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Кофе Латте",
                    ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE7D61B0C26A3F85D97A78FEEE00AD.webp",
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Сэнвич ветчина и сыр",
                    ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE796FF0059B799A17F57A9E64C725.webp",
                    CategoryId = 3
                },
                new Product
                {
                    Name = "Куриные наггетсы",
                    ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE7D618B5C7EC29350069AE9532C6E.webp",
                    CategoryId = 3
                },
                new Product
                {
                    Name = "Картофель из печи с соусом 🌱",
                    ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EED646A9CD324C962C6BEA78124F19.webp",
                    CategoryId = 3
                },
                new Product
                {
                    Name = "Додстер",
                    ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE796F96D11392A2F6DD73599921B9.webp",
                    CategoryId = 3
                },
                new Product
                {
                    Name = "Острый Додстер 🌶️🌶️",
                    ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE796FD3B594068F7A752DF8161D04.webp",
                    CategoryId = 3
                },
                new Product
                {
                    Name = "Банановый молочный коктейль",
                    ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EEE20B8772A72A9B60CFB20012C185.webp",
                    CategoryId = 4
                },
                new Product
                {
                    Name = "Карамельное яблоко молочный коктейль",
                    ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE79702E2A22E693D96133906FB1B8.webp",
                    CategoryId = 4
                },
                new Product
                {
                    Name = "Молочный коктейль с печеньем Оrео",
                    ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE796FA1F50F8F8111A399E4C1A1E3.webp",
                    CategoryId = 4
                },
                new Product
                {
                    Name = "Классический молочный коктейль 👶",
                    ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE796F93FB126693F96CB1D3E403FB.webp",
                    CategoryId = 4
                },
                new Product
                {
                    Name = "Ирландский Капучино",
                    ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE7D61999EBDA59C10E216430A6093.webp",
                    CategoryId = 5
                },
                new Product
                {
                    Name = "Кофе Карамельный капучино",
                    ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE7D61AED6B6D4BFDAD4E58D76CF56.webp",
                    CategoryId = 5
                },
                new Product
                {
                    Name = "Кофе Кокосовый латте",
                    ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE7D61B19FA07090EE88B0ED347F42.webp",
                    CategoryId = 5
                },
                new Product
                {
                    Name = "Кофе Американо",
                    ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE7D61B044583596548A59078BBD33.webp",
                    CategoryId = 5
                },
                new Product
                {
                    Name = "Кофе Латте",
                    ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE7D61B0C26A3F85D97A78FEEE00AD.webp",
                    CategoryId = 5
                }
            );
        }

        if (!context.Ingredients.Any())
        {
            context.Ingredients.AddRange(
                new Ingredient
                {
                    Name = "Сырный бортик",
                    Price = 179,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/99f5cb91225b4875bd06a26d2e842106.png"
                },
                new Ingredient
                {
                    Name = "Сливочная моцарелла",
                    Price = 79,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/cdea869ef287426386ed634e6099a5ba.png"
                },
                new Ingredient
                {
                    Name = "Сыры чеддер и пармезан",
                    Price = 79,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A22FA54A81411E9AFA69C1FE796"
                },
                new Ingredient
                {
                    Name = "Острый перец халапеньо",
                    Price = 59,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/11ee95b6bfdf98fb88a113db92d7b3df.png"
                },
                new Ingredient
                {
                    Name = "Нежный цыпленок",
                    Price = 79,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A39D824A82E11E9AFA5B328D35A"
                },
                new Ingredient
                {
                    Name = "Шампиньоны",
                    Price = 59,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A22FA54A81411E9AFA67259A324"
                },
                new Ingredient
                {
                    Name = "Ветчина",
                    Price = 79,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A39D824A82E11E9AFA61B9A8D61"
                },
                new Ingredient
                {
                    Name = "Пикантная пепперони",
                    Price = 79,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A22FA54A81411E9AFA6258199C3"
                },
                new Ingredient
                {
                    Name = "Острая чоризо",
                    Price = 79,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A22FA54A81411E9AFA62D5D6027"
                },
                new Ingredient
                {
                    Name = "Маринованные огурчики",
                    Price = 59,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A21DA51A81211E9EA89958D782B"
                },
                new Ingredient
                {
                    Name = "Свежие томаты",
                    Price = 59,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A39D824A82E11E9AFA7AC1A1D67"
                },
                new Ingredient
                {
                    Name = "Красный лук",
                    Price = 59,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A22FA54A81411E9AFA60AE6464C"
                },
                new Ingredient
                {
                    Name = "Сочные ананасы",
                    Price = 59,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A21DA51A81211E9AFA6795BA2A0"
                },
                new Ingredient
                {
                    Name = "Итальянские травы",
                    Price = 39,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/370dac9ed21e4bffaf9bc2618d258734.png"
                },
                new Ingredient
                {
                    Name = "Сладкий перец",
                    Price = 59,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A22FA54A81411E9AFA63F774C1B"
                },
                new Ingredient
                {
                    Name = "Кубики брынзы",
                    Price = 79,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A39D824A82E11E9AFA6B0FFC349"
                },
                new Ingredient
                {
                    Name = "Митболы",
                    Price = 79,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/b2f3a5d5afe44516a93cfc0d2ee60088.png"
                }
            );
        }

        context.SaveChanges();
    }
}