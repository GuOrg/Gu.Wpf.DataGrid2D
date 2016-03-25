# Gu.Wpf.DataGrid2D

[![Gitter Chat Room](https://img.shields.io/gitter/room/nwjs/nw.js.svg)](https://gitter.im/JohanLarsson/Gu.Wpf.DataGrid2D?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge) [![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md) [![NuGet](https://img.shields.io/nuget/v/Gu.Wpf.DataGrid2D.svg)](https://www.nuget.org/packages/Gu.Wpf.DataGrid2D/)
[![Build status](https://ci.appveyor.com/api/projects/status/a92oxrywc9nv7f21?svg=true)](https://ci.appveyor.com/project/JohanLarsson/gu-wpf-datagrid2d)

Extension methods for WPF DataGrid enabling binidng to sources of different types.

## ItemsSource.Array2D & Array2DTransposed
For binding to sources of type T[,]

##### Array2D
    <DataGrid HeadersVisibility="None"
              dataGrid2D:ItemsSource.Array2D="{Binding Data2D}" />

Renders:  
![ItemsSource2D render](http://i.imgur.com/00325df.png)

##### Explicit columns
Columns are referred to by `C<zero_based_index>`

```
<DataGrid AutoGenerateColumns="False"
          dataGrid2D:ItemsSource.Array2DTransposed="{Binding Data2D}">
    <DataGrid.Columns>
        <DataGridTextColumn Binding="{Binding C0}" Header="Col 1" />
        <DataGridTextColumn Binding="{Binding C1}" Header="Col 2" />
        <DataGridTextColumn Binding="{Binding C2}" Header="Col 3" />
    </DataGrid.Columns>
</DataGrid>
```
Renders:  
![ItemsSource2D render](http://i.imgur.com/IHvEI0c.png)

##### With headers:
```
<DataGrid dataGrid2D:ItemsSource.Array2D="{Binding Data2D}"
          dataGrid2D:ItemsSource.ColumnHeadersSource="{Binding ColumnHeaders}"
          dataGrid2D:ItemsSource.RowHeadersSource="{Binding RowHeaders}" />
```
Renders:  
![With headers screenie](http://i.imgur.com/GtEOW5G.png)

##### Array2DTransposed
```
<DataGrid dataGrid2D:ItemsSource.Array2DTransposed="{Binding Data2D}" />
```
Renders:  
![ItemsSource2D render](http://i.imgur.com/N6BJqIR.png)

## ItemsSource.RowsSource & ColumnsSource
Lets you bind to datasources of type `IEnumerable<IEnumerable>>`.
Tracks collection changes.

##### RowsSource
```
<DataGrid HeadersVisibility="None"
          dataGrid2D:ItemsSource.RowsSource="{Binding ListOfListsOfInts}" />
```

Renders:  
![ItemsSource2D render](http://i.imgur.com/00325df.png)

##### ColumnsSource
```
<DataGrid HeadersVisibility="None"
          dataGrid2D:ItemsSource.ColumnsSource="{Binding ListOfListsOfInts}" />
```
Renders:  
![ItemsSource2D render](http://i.imgur.com/N6BJqIR.png)

##### Different lengths
Limited support for different lengths. Columns with blanks are default readonly.

```
<DataGrid dataGrid2D:ItemsSource.RowsSource="{Binding DifferentLengths}" />
```

Renders:  
![ItemsSource2D render](http://i.imgur.com/PPlT750.png)

## Selected.CellItem & Index
Lets you two-way bind the item of the currently selected cell or index (row, col).
For this to work these conditions must be satisfied:
- `SelectionUnit="Cell"` 
- Columns must be of type `DataGridBoundColumn`. Don't think there is a way to dig out the bound property of a `DataGridTemplateColumn`
```
<DataGrid SelectionUnit="Cell"
          dataGrid2D:ItemsSource.RowsSource="{Binding RowVms}"
          dataGrid2D:Selected.CellItem="{Binding SelectedItem}"
          dataGrid2D:Selected.Index="{Binding Index}" />
``` 

## ItemsSource.TransposedSource & PropertySource
Support for transposing an itemssource, perhaps useful for property grid scenarios. Supports binding to single item or (Observable)Collection

##### PropertySource
Same as TransposedSource but for a single item.

<DataGrid dataGrid2D:ItemsSource.PropertySource="{Binding Person}">

Renders:  
![ItemsSource2D render](http://i.imgur.com/sn8VNKG.png)

##### TransposedSource with explicit columns
The property name column is named `Name` and the following columns are named `C<zero_based_index>`
```
<DataGrid AutoGenerateColumns="False" 
          dataGrid2D:ItemsSource.TransposedSource="{Binding Persons}">
    <DataGrid.Columns>
        <DataGridTextColumn Binding="{Binding Name}" Header="Property" />
        <DataGridTextColumn Binding="{Binding C0}" Header="Value 1" />
        <DataGridTextColumn Binding="{Binding C1}" Header="Value 2" />
    </DataGrid.Columns>
</DataGrid>
```

Renders:  
![ItemsSource2D render](http://i.imgur.com/ftkeyDu.png)

## Rownumbers
Conveninence attached property if you want to display rownumbers.
Specify the number to start fom using `StartAt` 
```
<DataGrid ItemsSource="{Binding Persons}" dataGrid2D:Index.StartAt="1">
    <DataGrid.RowHeaderStyle>
        <Style TargetType="{x:Type DataGridRowHeader}">
            <Setter Property="Content" Value="{Binding Path=(dataGrid2D:Index.OfRow), RelativeSource={RelativeSource AncestorType={x:Type DataGridRow}}}" />
        </Style>
    </DataGrid.RowHeaderStyle>
</DataGrid>
```
Renders:  
![Rownumbers render](http://i.imgur.com/VkDap9E.png)
