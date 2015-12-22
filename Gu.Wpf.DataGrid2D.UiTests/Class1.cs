namespace Gu.Wpf.DataGrid2D.UiTests
{
    using System.Linq;
    using System.Windows.Automation;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.Finders;

    public static class UIItemContainerExt
    {
        public static T GetById<T>(this UIItemContainer container, string id)
            where T : IUIItem
        {
            var criteria = SearchCriteria.ByAutomationId(id)
                                               .AndControlType(typeof(T), WindowsFramework.Wpf);
            if (container.HasItem<T>(id))
            {
                return (T)container.Get(criteria);
            }

            foreach (var item in container.Items.OfType<UIItemContainer>())
            {
                var byId = item.GetById<T>(id, criteria);
                if (byId != null)
                {
                    return (T)byId;
                }
            }

            return (T)container.Get(criteria); // so that white can throw the not found exception
        }

        private static T GetById<T>(this UIItemContainer container, string id, SearchCriteria criteria)
                        where T : IUIItem
        {
            if (container.HasItem<T>(id))
            {
                return (T)container.Get(criteria);
            }

            foreach (var item in container.Items.OfType<UIItemContainer>())
            {
                item.GetById<T>(id, criteria);
            }

            return default(T);
        }

        private static bool HasItem<T>(this UIItemContainer container, string id)
            where T : IUIItem
        {
            return container.Items.OfType<T>()
                            .Any(x => x.Id == id);
        }
    }
}
