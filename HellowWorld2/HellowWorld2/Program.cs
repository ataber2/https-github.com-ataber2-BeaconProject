using System;
using System.Collections.Generic;

namespace HellowWorld2
{
    public class Strings
    {

        static void Main(string[] args)
        {
            Dog animal1 = new Dog();
            Cat animal2 = new Cat();
            Duck animal3 = new Duck();
            animal1.sayHello();
            animal2.sayHello();
            animal3.sayHello();
        }
    }

    class Dog
    {
        public void sayHello()
        {
            Console.WriteLine("Bark");
        }
    }

    class Cat
    {
        public void sayHello()
        {
            Console.WriteLine("Meow");
        }
    }

    class Duck
    {
        public void sayHello()
        {
            Console.WriteLine("Quack");
        }
    }
}