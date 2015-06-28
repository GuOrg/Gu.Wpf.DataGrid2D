# Gu.Wpf.DataGrid2D
Extension methods for WPF DataGrid enabling binidng to T[,]

Sample where Data2D is int[,]

    <DataGrid HeadersVisibility="None"
              SelectionUnit="Cell"
              dataGrid2D:Source2D.ItemsSource2D="{Binding Data2D}" />

Renders:
![ItemsSource2D render](http://i.imgur.com/00325df.png)

With headers:
```
<DataGrid ColumnWidth="SizeToHeader"
          SelectionUnit="Cell"
          dataGrid2D:Source2D.ColumnHeadersSource="{Binding ColumnHeaders}"
          dataGrid2D:Source2D.ItemsSource2D="{Binding Data2D}" />
```
Renders:
![ItemsSource2D render](http://i.imgur.com/X1kTmUV.png)

Sample where ListOfListsOfItems is ```List<List<ItemVm>>```
```
<DataGrid SelectionUnit="Cell"
          HeadersVisibility="None"
          dataGrid2D:Source2D.RowsSource="{Binding ListOfListsOfItems}" />
```

Renders:
![ItemsSource2D render](http://i.imgur.com/UNQsW3q.png)

```SelectedCellItem``` lets you bind the item of the currently selected cell.

Support for styles & templates:
```
<DataGrid Grid.Column="2"
          Background="{x:Null}"
          CellStyle="{StaticResource CellStyle}"
          ColumnHeaderStyle="{StaticResource ColumnHeaderStyle}"
          ColumnWidth="*"
          RowBackground="{x:Null}"
          SelectionUnit="Cell"
          SelectionMode="Extended"
          dataGrid2D:Source2D.CellEditingTemplate="{StaticResource SampleItemEditTemplate}"
          dataGrid2D:Source2D.CellTemplate="{StaticResource SampleItemTemplate}"
          dataGrid2D:Source2D.ColumnHeadersSource="{Binding ColumnItemHeaders}"
          dataGrid2D:Source2D.HeaderTemplate="{StaticResource SampleHeaderItemTemplate}"
          dataGrid2D:Source2D.RowsSource="{Binding RowVms}"
          dataGrid2D:Source2D.SelectedCellItem="{Binding SelectedItem}">
    <DataGrid.RowHeaderStyle>
        <Style TargetType="DataGridRowHeader">
            <Setter Property="Content" Value="{Binding Name}" />
            <Setter Property="FontWeight" Value="Bold" />
            <Setter Property="Background" Value="Gainsboro" />
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="{x:Type DataGridRowHeader}">
                        <ContentPresenter Margin="1" />
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>
    </DataGrid.RowHeaderStyle>
</DataGrid>
```
Renders:
![ItemsSource2D render](http://i.imgur.com/qSKJ8Ga.png)

