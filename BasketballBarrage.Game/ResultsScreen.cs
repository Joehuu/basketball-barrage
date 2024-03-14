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
    private readonly GameplayMode mode;
    private readonly DateTime finishedTime;
    private readonly bool isTwoPlayers;
    private FillFlowContainer flow = null!;
    private readonly int[] pointsArray;

    public ResultsScreen(GameplayMode mode, int[] pointsArray, bool isTwoPlayers)
    {
        this.mode = mode;
        this.pointsArray = pointsArray;
        this.isTwoPlayers = isTwoPlayers;

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
            flow = new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(10),
            }
        };

        flow.Add(createOptions());

        if (isTwoPlayers)
            flow.Add(createOptions(true));
    }

    private Drawable createOptions(bool isPlayerTwo = false)
    {
        GameTextBox nameTextBox;
        GameButton submitButton;

        string playerLabel = isPlayerTwo ? "P2" : "P1";

        var options = new FillFlowContainer
        {
            AutoSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Direction = FillDirection.Vertical,
            Spacing = new Vector2(25),
            Children = new Drawable[]
            {
                new StatisticCounter($"{playerLabel} Points")
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    CounterValue = { BindTarget = new Bindable<int>(pointsArray[isPlayerTwo ? 1 : 0]) },
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
                    Text = "Retry",
                    Action = () =>
                    {
                        ValidForResume = false;
                        this.Push(new GameplayScreen(mode, isTwoPlayers));
                    },
                },
                new GameButton
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Text = "Back",
                    Action = this.Exit
                },
            }
        };

        nameTextBox.OnCommit += (_, _) => submitButton.TriggerClick();
        submitButton.Action = () =>
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text)) return;

            submitScore(nameTextBox.Text, isPlayerTwo);
            submitButton.Enabled.Value = false;
        };

        return options;
    }

    private void submitScore(string playerName, bool isPlayerTwo)
    {
        var realm = Realm.GetInstance($"{Directory.GetCurrentDirectory()}/client.realm");

        realm.Write(() =>
        {
            realm.Add(new Score
            {
                PlayerName = playerName,
                Points = pointsArray[isPlayerTwo ? 1 : 0],
                Mode = mode.ToString(),
                Timestamp = finishedTime.ToString(CultureInfo.InvariantCulture),
            });
        });
    }
}
