using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp3
{
    public class Product
    {
        public string Name;
        public decimal Price;
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public void Print()
        {
            Console.WriteLine($"{Name} {Price}");
        }
    }

    public class Order
    {
        public List<Product> Products;
        public decimal FullPrice;
        public Order(List<Product> products)
        {
            Products = products;

            foreach (var product in products)
            {
                FullPrice += product.Price;
            }
        }
    }

    public class Store
    {
        public List<Product> Products;

        public List<Product> Basket;

        public List<Order> Orders;

        public List<User> Users;
        public Store()
        {
            Products = new List<Product>
            {
                new Product("Хлеб", 25),
                new Product("Молоко", 100),
                new Product("Печенье", 50),
                new Product("Масло", 250),
                new Product("Йогурт", 300),
                new Product("Сок", 80)
            };

            Basket = new List<Product>();
            Orders = new List<Order>();
            Users = new List<User>();
        }

        public void ShowCatalog()
        {
            Console.WriteLine("Каталог продуктов");
            ShowProducts(Products);
        }

        public void ShowBasket()
        {
            Console.WriteLine("Содержимое корзины");
            ShowProducts(Basket);
        }

        public void AddToBasket(int numberProduct)
        {
            Basket.Add(Products[numberProduct - 1]);
            Console.WriteLine($"Продукт {Products[numberProduct - 1].Name} успешно добавлен в корзину.");
            Console.WriteLine($"В корзине {Basket.Count} продуктов.");
        }

        public void ShowProducts(List<Product> products)
        {
            int number = 1;
            foreach (Product product in products)
            {
                Console.Write(number + ". ");
                product.Print();
                number++;
            }
        }

        public void CreateOrder()
        {
            Order order = new Order(Basket);
            Orders.Add(order);

            Basket.Clear();
        }

        public void UserGoods(User user)
        {
            int id = user.Id ;
            string userName = user.Name;
            User userNew = new User(id, userName, Orders);
            Users.Add(userNew);
            Console.WriteLine($"Заказ сформирован.");
        }

        public void UserGoodsPrint(User user)
        {
            Console.WriteLine($"Заказ № 12345 сформирован на имя {user.Name}");
        }
    }

    public class Admin
    {
        public int Id;
        public string Name;
        public Admin()
        {

        }
        public Admin(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddNewProduct(Store store) 
        {
            Console.WriteLine("Введите название товара, который будет добавлен в каталог");
            string name = Console.ReadLine();
            Console.WriteLine("Введите цену товара, который будет добавлен в каталог");
            decimal price = Convert.ToDecimal(Console.ReadLine());
            Product prod = new Product(name, price);
            store.Products.Add(prod);
            Console.WriteLine("Товар успешно добавлен в каталог");
        }
    }

    public class User
    {
        public int Id;
        public string Name;
        public List<Order> ProductsInUser;

        public User()
        {

        }

        public User(int id, string name, List<Order> orders)
        {
            Id = id;
            Name = name;
            ProductsInUser = orders;
        }
    }

    class Program
    {
        static void Main()
        {
            Store onlineStore = new Store();

            Console.WriteLine("Введите Ваше имя");
            string name = Console.ReadLine();

            Console.WriteLine("Введите Ваш ID");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Введите Вашу роль - admin или user");
            Admin admin = new Admin();
            User user = new User();
            string role = Console.ReadLine();
            if (role.ToLower() == "admin")
            {
                admin = new Admin(id, name);
                Console.WriteLine("Аккаунт админа создан");
            }
            else if (role.ToLower() == "user")
            {
                user = new User(id, name, onlineStore.Orders);
                Console.WriteLine("Аккаунт пользователя создан");
            }
            else throw new Exception("Не корректный ввод роли!");
            int numberAction = 0;
            bool yes;
            int numberProduct;
            while (numberAction != 5) 
            {
                Console.WriteLine("Выберите действие введя цифру");
                Console.WriteLine("1.Показать каталог продуктов");
                Console.WriteLine("2.Добавить продукты в корзину");
                Console.WriteLine("3.Оформление заказа");
                Console.WriteLine("4.Добавить продукт в каталог (для админа) !");
                Console.WriteLine("5.Покинуть магазин");

                try { numberAction = Convert.ToInt32(Console.ReadLine()); }
                catch (IOException e) { Console.WriteLine("Введено не число"); }
                if (numberAction >= 1 && numberAction <= 5 )
                {
                    Console.WriteLine("OK");
                }
                else Console.WriteLine("Некорректно введена команда, введите правильную команду! от 1 до 5");
                switch (numberAction)
                {
                    case 1: onlineStore.ShowCatalog(); 
                        break;
                    case 2:
                        onlineStore.ShowCatalog();
                        do
                        {
                            Console.Write("Напишите номер продукта, который нужно добавить: ");
                            numberProduct = Convert.ToInt32(Console.ReadLine());
                            onlineStore.AddToBasket(numberProduct);

                            Console.WriteLine("Добавить еще продукты? да/нет ");
                            yes = IsYes(Console.ReadLine());

                        } while (yes);
                        Console.WriteLine("Хотите посмотреть корзину? да/нет: ");
                        yes = IsYes(Console.ReadLine());

                        if (yes)
                        {
                            onlineStore.ShowBasket();
                        }; 
                        break;

                    case 3:
                        onlineStore.CreateOrder();
                        onlineStore.UserGoods(user);
                        onlineStore.UserGoodsPrint(user);
                        break;
                    case 4: 
                        admin.AddNewProduct(onlineStore); 
                        break;
                    case 5: 
                        break;
                    default: Console.WriteLine("Вы ничего не выбрали"); break;
                }

            }
        }
        static bool IsYes(string answer)
        {
            return answer.ToLower() == "да";
        }
    }
}
