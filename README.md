The project shows up issues related to the use of a custom grouping and sorting logic with the WPF DataGrid component.

The issued hadn't been appeared until I introduced my own SlidePropertyGroupDescription class (that inherits from GroupDescription). Since then different sorts of issues have been occurring (quite ofter) whenever I add or remove items to the DataGrid.

It's perfectly reproducible by adding and removing the same "magazine" in turn when the item collection is initially empty.
