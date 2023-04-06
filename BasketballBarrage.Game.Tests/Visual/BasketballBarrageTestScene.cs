using osu.Framework.Testing;

namespace BasketballBarrage.Game.Tests.Visual
{
    public abstract partial class BasketballBarrageTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new BasketballBarrageTestSceneTestRunner();

        private partial class BasketballBarrageTestSceneTestRunner : BasketballBarrageGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}
