using System;
using System.Globalization;
using System.IO;
using BasketballBarrage.Game.Database;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osuTK;
using Realms;

namespace BasketballBarrage.Game;

public partial class ResultsScreen : GameScreen
{
    public IBindable<int> Points = new Bindable<int>();
    private GameTextBox nameTextBox = null!;
    private GameButton submitButton = null!;
    private readonly GameplayMode mode;
    private readonly DateTime finishedTime;

    public ResultsScreen(GameplayMode mode)
    {
        this.mode = mode;

        finishedTime = DateTime.Now;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.DimGray,
            },
            new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(25),
                Children = new Drawable[]
                {
                    new StatisticCounter("Points")
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        CounterValue = { BindTarget = Points },
                    },
                    nameTextBox = new GameTextBox
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        PlaceholderText = "Name",
                        Size = new Vector2(200, 50),
                    },
                    submitButton = new GameButton
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Text = "Submit",
                        Action = submitScore
                    },
                    new GameButton
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Text = "Leaderboards",
                        Action = () => this.Push(new LeaderboardScreen()),
                    },
                    new GameButton
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Text = "Back",
                        Action = this.Exit
                    },
                }
            }
        };

        nameTextBox.OnCommit += (_, _) => submitButton.TriggerClick();
    }

    private void submitScore()
    {
        if (string.IsNullOrWhiteSpace(nameTextBox.Text)) return;

        var realm = Realm.GetInstance($"{Directory.GetCurrentDirectory()}/client.realm");

        realm.Write(() =>
        {
            realm.Add(new Score
            {
                PlayerName = nameTextBox.Text,
                Points = Points.Value,
                Mode = mode.ToString(),
                Timestamp = finishedTime.ToString(CultureInfo.InvariantCulture),
            });
        });

        submitButton.Enabled.Value = false;
    }
}
