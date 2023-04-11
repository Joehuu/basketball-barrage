using System.IO;
using System.Linq;
using BasketballBarrage.Game.Database;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using Realms;

namespace BasketballBarrage.Game;

public partial class LeaderboardScreen : GameScreen
{
    private FillFlowContainer flow = null!;
    private FillFlowContainer nameColumn = null!;
    private FillFlowContainer pointColumn = null!;
    private FillFlowContainer modeColumn = null!;
    private FillFlowContainer timeColumn = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Black,
            },
            flow = new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(25),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Child = new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Horizontal,
                    Spacing = new Vector2(25),
                    Children = new Drawable[]
                    {
                        nameColumn = new ColumnFlowContainer(),
                        pointColumn = new ColumnFlowContainer(),
                        modeColumn = new ColumnFlowContainer(),
                        timeColumn = new ColumnFlowContainer(),
                    }
                }
            },
        };

        var realm = Realm.GetInstance($"{Directory.GetCurrentDirectory()}/client.realm");

        foreach (var score in realm.All<Score>().OrderByDescending(s => s.Points))
        {
            nameColumn.Add(new SpriteText
            {
                Text = score.PlayerName,
            });
            pointColumn.Add(new SpriteText
            {
                Text = score.Points.ToString(),
            });
            modeColumn.Add(new SpriteText
            {
                Text = score.Mode,
            });
            timeColumn.Add(new SpriteText
            {
                Text = score.Timestamp,
            });
        }

        flow.Add(new GameButton
        {
            Anchor = Anchor.TopCentre,
            Origin = Anchor.TopCentre,
            Text = "Back",
            Action = this.Exit
        });
    }

    private partial class ColumnFlowContainer : FillFlowContainer
    {
        public ColumnFlowContainer()
        {
            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;
            AutoSizeAxes = Axes.Both;
            Direction = FillDirection.Vertical;
            Spacing = new Vector2(25);
        }
    }
}
