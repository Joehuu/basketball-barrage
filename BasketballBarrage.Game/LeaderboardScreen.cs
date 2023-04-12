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
    private GameScrollContainer scrollContainer = null!;

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
            new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(),
                    new Dimension(GridSizeMode.AutoSize),
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        scrollContainer = new GameScrollContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Padding = new MarginPadding(25),
                        }
                    },
                    new Drawable[]
                    {
                        new Container
                        {
                            AutoSizeAxes = Axes.Both,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Padding = new MarginPadding(25),
                            Child = new GameButton
                            {
                                Text = "Back",
                                Action = this.Exit
                            }
                        }
                    }
                }
            }
        };

        addContent();
    }

    private void addContent()
    {
        var realm = Realm.GetInstance($"{Directory.GetCurrentDirectory()}/client.realm");

        var scores = realm.All<Score>().OrderByDescending(s => s.Points);

        if (scores.Any())
        {
            FillFlowContainer nameColumn;
            FillFlowContainer pointColumn;
            FillFlowContainer modeColumn;
            FillFlowContainer timeColumn;

            scrollContainer.Child = new FillFlowContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
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
            };

            foreach (var score in scores)
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
        }
        else
        {
            scrollContainer.Child = new SpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "No scores yet!",
            };
        }
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
