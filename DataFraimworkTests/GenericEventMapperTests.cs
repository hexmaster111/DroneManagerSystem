using System;
using GenericEventMapper;
using GenericMessaging;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace ProjectTests;

public class GenericEventMapperTests
{
    private ConsoleLog.ConsoleLog logger;
    private EventMapper mapper;
    private Action<SendableTarget> action;

    private class UnitTestMessage : ISendable
    {
        public UnitTestMessage(string message, int number, byte byt, bool b, float f, double d, long l, short s, char c,
            decimal @decimal, DateTime dateTime, TimeSpan timeSpan, Guid guid, Complex complexObject)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Number = number;
            Byte = byt;
            Bool = b;
            Float = f;
            Double = d;
            Long = l;
            Short = s;
            Char = c;
            Decimal = @decimal;
            DateTime = dateTime;
            TimeSpan = timeSpan;
            Guid = guid;
            ComplexObject = complexObject ?? throw new ArgumentNullException(nameof(complexObject));
        }

        public string Message { get; set; }
        public int Number { get; set; }
        public byte Byte { get; set; }
        public bool Bool { get; set; }
        public float Float { get; set; }
        public double Double { get; set; }
        public long Long { get; set; }
        public short Short { get; set; }
        public char Char { get; set; }
        public decimal Decimal { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public Guid Guid { get; set; }

        public class Complex
        {
            public Complex(int number, int number2)
            {
                Number = number;
                Number2 = number2;
            }

            public int Number { get; set; }
            public int Number2 { get; set; }
        }

        public Complex ComplexObject { get; set; }


        public JObject ToJson()
        {
            return JObject.FromObject(this);
        }
    }

    [SetUp]
    public void Setup()
    {
        logger = new ConsoleLog.ConsoleLog();
        mapper = new EventMapper(logger);
        action += mapper.HandleEvent;
    }

    [Test]
    public void BasicMapTest()
    {
        var testItem = new UnitTestMessage("Hello World", 1, 2, true, 3.4f, 5.6, 7, 8, 'c', 9.10m, DateTime.Now,
            TimeSpan.FromMinutes(1), Guid.NewGuid(), new UnitTestMessage.Complex(1, 2));
        var message = new SendableTarget(testItem, "UnitTestMessage");


        mapper.MapAction<UnitTestMessage>("UnitTestMessage", testMessage =>
        {
            if (testMessage.Message == testItem.Message && testMessage.Number == testItem.Number &&
                testMessage.Byte == testItem.Byte && testMessage.Bool == testItem.Bool &&
                Math.Abs(testMessage.Float - testItem.Float) < .01 &&
                Math.Abs(testMessage.Double - testItem.Double) < .01 &&
                testMessage.Long == testItem.Long && testMessage.Short == testItem.Short &&
                testMessage.Char == testItem.Char && testMessage.Decimal == testItem.Decimal &&
                testMessage.DateTime == testItem.DateTime && testMessage.TimeSpan == testItem.TimeSpan &&
                testMessage.Guid == testItem.Guid &&
                testMessage.ComplexObject.Number == testItem.ComplexObject.Number &&
                testMessage.ComplexObject.Number2 == testItem.ComplexObject.Number2)
                Assert.Pass();
            else
                Assert.Fail();
        });


        action.Invoke(message);


        Assert.Fail();
    }

    private class SimpleTestClass : ISendable
    {
        public int A;
        public int B;
        public string C;

        public SimpleTestClass(int a, int b, string c) 
        {
            A = a;
            B = b;
            C = c;
        }

        public JObject ToJson()
        {
            return JObject.FromObject(this);
        }
    }
    
    [Test]
    public void TestRegister()
    {
        bool actionRan = false;
        mapper.MapAction<SimpleTestClass>("RegTest", (simpleTest) =>
        {
            actionRan = true;
        });
        
        action.Invoke(new SendableTarget(new SimpleTestClass(1,2,"3"),"RegTest"));
        
        if(actionRan)
            Assert.Pass();
        else
            Assert.Fail();
    }
    
    [Test]
    public void Unregister()
    {
        bool actionRan = false;
        
        mapper.MapAction<SimpleTestClass>("UnregTest", (simpleTest) =>
        {
            actionRan = true;
        });
        
        
        
        action.Invoke(new SendableTarget(new SimpleTestClass(1,2,"3"),"UnregTest"));
        
        if(!actionRan) //The invoke failed to run 
            Assert.Fail();
        
        actionRan = false;
        mapper.UnregisterAction("UnregTest");
        
        action.Invoke(new SendableTarget(new SimpleTestClass(1,2,"3"),"UnregTest"));
        
        if(actionRan)
            Assert.Fail(); //The invoke ran after unregistering
        else
            Assert.Pass();
    }
}