public static class SeedData
{
    private static double RandomDecimalNumber(int min, int max)
    {
        Random rnd = new Random();
        double value = rnd.Next(min, max);
        return Math.Floor(value);
    }

    private static ProductItem GenerateProductItem(Product product, int? pizzaType = null, int? size = null)
    {
        return new ProductItem
        {
            Product = product,
            Price = RandomDecimalNumber(190, 600),
            PizzaType = pizzaType,
            Size = size
        };
    }

    public static void EnsurePopulated(IApplicationBuilder app)
    {
        PizzaDb context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<PizzaDb>();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        var ingredients = new List<Ingredient>{
            new Ingredient
                {
                    Name = "Сырный бортик",
                    Price = 179,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/99f5cb91225b4875bd06a26d2e842106.png",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Ingredient
                {
                    Name = "Сливочная моцарелла",
                    Price = 79,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/cdea869ef287426386ed634e6099a5ba.png",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Ingredient
                {
                    Name = "Сыры чеддер и пармезан",
                    Price = 79,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A22FA54A81411E9AFA69C1FE796",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Ingredient
                {
                    Name = "Острый перец халапеньо",
                    Price = 59,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/11ee95b6bfdf98fb88a113db92d7b3df.png",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Ingredient
                {
                    Name = "Нежный цыпленок",
                    Price = 79,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A39D824A82E11E9AFA5B328D35A",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Ingredient
                {
                    Name = "Шампиньоны",
                    Price = 59,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A22FA54A81411E9AFA67259A324",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Ingredient
                {
                    Name = "Ветчина",
                    Price = 79,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A39D824A82E11E9AFA61B9A8D61",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Ingredient
                {
                    Name = "Пикантная пепперони",
                    Price = 79,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A22FA54A81411E9AFA6258199C3",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Ingredient
                {
                    Name = "Острая чоризо",
                    Price = 79,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A22FA54A81411E9AFA62D5D6027",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Ingredient
                {
                    Name = "Маринованные огурчики",
                    Price = 59,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A21DA51A81211E9EA89958D782B",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Ingredient
                {
                    Name = "Свежие томаты",
                    Price = 59,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A39D824A82E11E9AFA7AC1A1D67",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Ingredient
                {
                    Name = "Красный лук",
                    Price = 59,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A22FA54A81411E9AFA60AE6464C",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Ingredient
                {
                    Name = "Сочные ананасы",
                    Price = 59,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A21DA51A81211E9AFA6795BA2A0",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Ingredient
                {
                    Name = "Итальянские травы",
                    Price = 39,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/370dac9ed21e4bffaf9bc2618d258734.png",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Ingredient
                {
                    Name = "Сладкий перец",
                    Price = 59,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A22FA54A81411E9AFA63F774C1B",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Ingredient
                {
                    Name = "Кубики брынзы",
                    Price = 79,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/000D3A39D824A82E11E9AFA6B0FFC349",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Ingredient
                {
                    Name = "Митболы",
                    Price = 79,
                    ImageUrl = "https://cdn.dodostatic.net/static/Img/Ingredients/b2f3a5d5afe44516a93cfc0d2ee60088.png",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
        };

        var product1 = new Product
        {
            Name = "Омлет с ветчиной и грибами",
            ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE7970321044479C1D1085457A36EB.webp",
            CategoryId = 2,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var product2 = new Product
        {
            Name = "Омлет с пепперони",
            ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE94ECF33B0C46BA410DEC1B1DD6F8.webp",
            CategoryId = 2,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var product3 = new Product
        {
            Name = "Кофе Латте",
            ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE7D61B0C26A3F85D97A78FEEE00AD.webp",
            CategoryId = 2,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var product4 = new Product
        {
            Name = "Сэнвич ветчина и сыр",
            ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE796FF0059B799A17F57A9E64C725.webp",
            CategoryId = 3,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var product5 = new Product
        {
            Name = "Куриные наггетсы",
            ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE7D618B5C7EC29350069AE9532C6E.webp",
            CategoryId = 3,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var product6 = new Product
        {
            Name = "Картофель из печи с соусом 🌱",
            ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EED646A9CD324C962C6BEA78124F19.webp",
            CategoryId = 3,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var product7 = new Product
        {
            Name = "Додстер",
            ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE796F96D11392A2F6DD73599921B9.webp",
            CategoryId = 3,
            Ingredients = ingredients.Skip(0).Take(5).ToList(),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var product8 = new Product
        {
            Name = "Острый Додстер 🌶️🌶️",
            ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE796FD3B594068F7A752DF8161D04.webp",
            CategoryId = 3,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var product9 = new Product
        {
            Name = "Банановый молочный коктейль",
            ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EEE20B8772A72A9B60CFB20012C185.webp",
            CategoryId = 4,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var product10 = new Product
        {
            Name = "Карамельное яблоко молочный коктейль",
            ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE79702E2A22E693D96133906FB1B8.webp",
            CategoryId = 4,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var product11 = new Product
        {
            Name = "Молочный коктейль с печеньем Оrео",
            ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE796FA1F50F8F8111A399E4C1A1E3.webp",
            CategoryId = 4,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var product12 = new Product
        {
            Name = "Классический молочный коктейль 👶",
            ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE796F93FB126693F96CB1D3E403FB.webp",
            CategoryId = 4,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var product13 = new Product
        {
            Name = "Ирландский Капучино",
            ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE7D61999EBDA59C10E216430A6093.webp",
            CategoryId = 5,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var product14 = new Product
        {
            Name = "Кофе Карамельный капучино",
            ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE7D61AED6B6D4BFDAD4E58D76CF56.webp",
            CategoryId = 5,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var product15 = new Product
        {
            Name = "Кофе Кокосовый латте",
            ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE7D61B19FA07090EE88B0ED347F42.webp",
            CategoryId = 5,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var product16 = new Product
        {
            Name = "Кофе Американо",
            ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE7D61B044583596548A59078BBD33.webp",
            CategoryId = 5,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var product17 = new Product
        {
            Name = "Кофе Латте",
            ImageUrl = "https://media.dodostatic.net/image/r:292x292/11EE7D61B0C26A3F85D97A78FEEE00AD.webp",
            CategoryId = 5,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        if (!context.Products.Any())
        {
            context.Products.AddRange(product1, product2, product3, product4, product5, product6, product7, product8, product9, product10, product11, product12, product13, product14, product15, product16, product17);
        }

        if (!context.Ingredients.Any())
        {
            context.Ingredients.AddRange(ingredients);
        }

        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category
                {
                    Name = "Пиццы"
                },
                new Category
                {
                    Name = "Комбо"
                },
                new Category
                {
                    Name = "Закуски"
                },
                new Category
                {
                    Name = "Коктейли"
                },
                new Category
                {
                    Name = "Кофе"
                },
                new Category
                {
                    Name = "Напитки"
                },
                new Category
                {
                    Name = "Десерты"
                },
                new Category
                {
                    Name = "Деликатесы"
                }
            );
        }

        Product pizza1 = new Product
        {
            Name = "Пепперони фреш",
            ImageUrl = "https://media.dodostatic.net/image/r:233x233/11EE7D61304FAF5A98A6958F2BB2D260.webp",
            CategoryId = 1,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Ingredients = ingredients.Skip(0).Take(5).ToList()
        };

        var productItem = GenerateProductItem(pizza1, 1, 20);

        if (!context.ProductItems.Any())
        {
            Product pizza2 = new Product
            {
                Name = "Пепперони фреш",
                ImageUrl = "https://media.dodostatic.net/image/r:233x233/11EE7D61304FAF5A98A6958F2BB2D260.webp",
                CategoryId = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Ingredients = ingredients.Skip(0).Take(5).ToList()
            };

            Product pizza3 = new Product
            {
                Name = "Чоризо фреш",
                ImageUrl = "https://media.dodostatic.net/image/r:584x584/11EE7D61706D472F9A5D71EB94149304.webp",
                CategoryId = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Ingredients = ingredients.Skip(10).Take(40).ToList()
            };

            context.Products.AddRange(pizza1, pizza2, pizza3);

            context.ProductItems.AddRange(
                // Пицца "Пепперони фреш"
                productItem,
                GenerateProductItem(pizza1, 2, 30),
                GenerateProductItem(pizza1, 2, 40),

                // Пицца "Сырная"
                GenerateProductItem(pizza2, 1, 20),
                GenerateProductItem(pizza2, 1, 30),
                GenerateProductItem(pizza2, 1, 40),
                GenerateProductItem(pizza2, 2, 20),
                GenerateProductItem(pizza2, 2, 30),
                GenerateProductItem(pizza2, 2, 40),

                // Пицца "Чоризо фреш"
                GenerateProductItem(pizza3, 1, 20),
                GenerateProductItem(pizza3, 2, 30),
                GenerateProductItem(pizza3, 2, 40),

                // Остальные продукты
                GenerateProductItem(product1),
                GenerateProductItem(product2),
                GenerateProductItem(product3),
                GenerateProductItem(product4),
                GenerateProductItem(product5),
                GenerateProductItem(product6),
                GenerateProductItem(product7),
                GenerateProductItem(product8),
                GenerateProductItem(product9),
                GenerateProductItem(product10),
                GenerateProductItem(product11),
                GenerateProductItem(product12),
                GenerateProductItem(product13),
                GenerateProductItem(product14),
                GenerateProductItem(product15),
                GenerateProductItem(product16),
                GenerateProductItem(product17)
            );
        }


        if (!context.Stores.Any())
        {
            context.Stores.AddRange(
                new Story
                {
                    PreviewImageUrl = "https://cdn.inappstory.ru/story/xep/xzh/zmc/cr4gcw0aselwvf628pbmj3j/custom_cover/logo-350x440.webp?k=IgAAAAAAAAAE&v=3101815496"
                },
                new Story
                {
                    PreviewImageUrl = "https://cdn.inappstory.ru/story/km2/9gf/jrn/sb7ls1yj9fe5bwvuwgym73e/custom_cover/logo-350x440.webp?k=IgAAAAAAAAAE&v=3074015640"
                },
                new Story
                {
                    PreviewImageUrl = "https://cdn.inappstory.ru/story/quw/acz/zf5/zu37vankpngyccqvgzbohj1/custom_cover/logo-350x440.webp?k=IgAAAAAAAAAE&v=1336215020"
                },
                new Story
                {
                    PreviewImageUrl = "https://cdn.inappstory.ru/story/7oc/5nf/ipn/oznceu2ywv82tdlnpwriyrq/custom_cover/logo-350x440.webp?k=IgAAAAAAAAAE&v=38903958"
                },
                new Story
                {
                    PreviewImageUrl = "https://cdn.inappstory.ru/story/q0t/flg/0ph/xt67uw7kgqe9bag7spwkkyw/custom_cover/logo-350x440.webp?k=IgAAAAAAAAAE&v=2941222737"
                },
                new Story
                {
                    PreviewImageUrl = "https://cdn.inappstory.ru/story/lza/rsp/2gc/xrar8zdspl4saq4uajmso38/custom_cover/logo-350x440.webp?k=IgAAAAAAAAAE&v=4207486284"
                }
            );
        }

        if (!context.StoryItems.Any())
        {
            context.StoryItems.AddRange(
                new StoryItem
                {
                    StoryId = 1,
                    SourceUrl = "https://cdn.inappstory.ru/file/dd/yj/sx/oqx9feuljibke3mknab7ilb35t.webp?k=IgAAAAAAAAAE"
                },
                new StoryItem
                {
                    StoryId = 1,
                    SourceUrl = "https://cdn.inappstory.ru/file/jv/sb/fh/io7c5zarojdm7eus0trn7czdet.webp?k=IgAAAAAAAAAE"
                },
                new StoryItem
                {
                    StoryId = 1,
                    SourceUrl = "https://cdn.inappstory.ru/file/ts/p9/vq/zktyxdxnjqbzufonxd8ffk44cb.webp?k=IgAAAAAAAAAE"
                },
                new StoryItem
                {
                    StoryId = 1,
                    SourceUrl = "https://cdn.inappstory.ru/file/ur/uq/le/9ufzwtpdjeekidqq04alfnxvu2.webp?k=IgAAAAAAAAAE"
                },
                new StoryItem
                {
                    StoryId = 1,
                    SourceUrl = "https://cdn.inappstory.ru/file/sy/vl/c7/uyqzmdojadcbw7o0a35ojxlcul.webp?k=IgAAAAAAAAAE"
                }
            );
        }

        var user1 = new User
        {
            Login = "User",
            FullName = "User",
            Email = "user@test.ru",
            Password = "11111",
            Verified = DateTime.Now,
            Role = UserRole.USER
        };
        var user2 = new User
        {
            Login = "Admin",
            FullName = "Admin",
            Email = "admin@test.ru",
            Password = "22222",
            Verified = DateTime.Now,
            Role = UserRole.ADMIN
        };

        if (!context.Users.Any())
        {
            context.Users.AddRange(user1, user2);
        }

        var cart = new Cart
        {
            User = user1,
            TokenId = "1111",
            TotalAmount = 650,
            CreatedAt = DateTime.Now
        };

        if (!context.Carts.Any())
        {
            context.Carts.AddRange(
                cart,
                new Cart
                {
                    User = user2,
                    TokenId = "2222",
                    TotalAmount = 750,
                    CreatedAt = DateTime.Now
                }
            );
        }

        if (!context.CartItems.Any())
        {
            context.CartItems.Add(
                new CartItem
                {
                    ProductItem = productItem,
                    Cart = cart,
                    User = user1,
                    Quantity = 1,
                    Ingredients = ingredients.Skip(0).Take(5).ToList()
                }
            );
        }

        context.SaveChanges();
    }
}