using LiveSortingIssues.DataModel;

namespace LiveSortingIssues.Components.Slides;

public readonly record struct SlidePresenceUpdate(Slide Slide, SlidePresenceUpdateType UpdateType);
