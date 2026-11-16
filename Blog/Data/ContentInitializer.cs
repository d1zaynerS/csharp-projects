using Blog.Models;

namespace Blog.Data
{
    public class ContentInitializer
    {

        public static async Task InitializeAsync(ApplicationContext context)

        {

            if (!context.Categories.Any())

            {

                context.Categories.AddRange(

                        new Category { Name = "Подорожі", Description = "Подорожі по всій земній кулі." },

                        new Category { Name = "Історії", Description = "Розказана історія - це прожите життя." },

                        new Category { Name = "Фільми", Description = "Все! Кіна не буде! Електрика скінчилася!" }

                    );

                context.SaveChanges();

            }



            if (!context.Publications.Any())

            {

                context.Publications.AddRange(

                        new Publication
                        {

                            Title = "Детройт: хроніки «мертвого» міста",

                            Description = "А вам слабо взяти та експромтом поїхати до Детройта, штат Мічиган на автосалон, бо друзі покликали? Нам виявилося не слабко. Залишали дрібнички в машину та поїхали. І ця поїздка несподівано виявилася епічною від початку до кінця. А, може, очікувано, бо місто все-таки, мабуть, не просте. Вже від багатьох, хто коментував мої пости про цю поїздку до Фейсбуки, чула, що там без пригод не можна 🙂",

                            Categories = new List<Category>()

                            {

                            context.Categories.FirstOrDefault(e => e.Name.Equals("Подорожі"))

                            },

                            SeoDescription = "Детройт: хроніки «мертвого» міста",

                            Keywords = "поїздка до детройту"
                        },

                        new Publication
                        {

                            Title = "Досягти успіху, змінюючи образ",

                            Description = "Коли в місті зміцнилася висока думка про чесноти та знання молодого «священнослужителя», його без заперечень обрали татом. На жаль, природа зрештою взяла своє і «папа» опинився в цікавому положенні.",

                            Categories = new List<Category>()

                            {

                            context.Categories.FirstOrDefault(e => e.Name.Equals("Історії")),

                            context.Categories.FirstOrDefault(e => e.Name.Equals("Подорожі"))

                            },

                            SeoDescription = "Досягти успіху, змінюючи образ. Цікава історія",

                            Keywords = "успіх у житті"
                        }

                    );

                context.SaveChanges();

            }

        }
    }
}
