using System.Linq;
using System.Threading;
using IsogradTestRunner.Extensions;
using IsogradTestRunner.Helpers;
using NFluent;
using NUnit.Framework;

namespace IsogradTestRunnerTests
{
    public class DebouncerTests
    {
        [Test]
        public void MustDebounceCallWithTheSameValueWhenTheDelayBetweenCallIsLowerThanDebouncerDelay()
        {
            var debouncer = new Debouncer<string>(_ => _counter++, 20.Milliseconds());

            debouncer.DebouncedActionFor("a");
            debouncer.DebouncedActionFor("a");

            Thread.Sleep(50.Milliseconds());

            Check.That(_counter).IsEqualTo(1);
        }

        [Test]
        public void MustNotDebounceCallWithTheSameValueWhenTheDelayBetweenCallIsGreaterhanDebouncerDelay()
        {
            var debouncer = new Debouncer<string>(_ => _counter++, 20.Milliseconds());

            debouncer.DebouncedActionFor("a");
            Thread.Sleep(50.Milliseconds());

            debouncer.DebouncedActionFor("a");
            Thread.Sleep(50.Milliseconds());

            Check.That(_counter).IsEqualTo(2);
        }

        [Test]
        public void MustDebounceCallWithTheSameProjectionValueWhenTheDelayBetweenCallIsLowerThanDebouncerDelay()
        {
            var debouncer = new DebouncerWithProjection<string, char>(_ => _counter++, x => x.First(), 20.Milliseconds());

            debouncer.DebouncedActionFor("a1");
            debouncer.DebouncedActionFor("a2");
            debouncer.DebouncedActionFor("a3");

            Thread.Sleep(50.Milliseconds());

            Check.That(_counter).IsEqualTo(1);
        }

        [Test]
        public void MustNotDebounceCallWithDifferentValues()
        {
            var debouncer = new Debouncer<string>(_ => _counter++, 20.Milliseconds());

            debouncer.DebouncedActionFor("a");
            debouncer.DebouncedActionFor("b");
            debouncer.DebouncedActionFor("c");

            Thread.Sleep(30.Milliseconds());
            Check.That(_counter).IsEqualTo(3);
        }

        private int _counter;

        [SetUp]
        public void ResetCounter()
        {
            _counter = 0;
        }
    }
}
