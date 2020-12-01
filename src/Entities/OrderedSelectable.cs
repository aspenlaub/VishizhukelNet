namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities {
    // ReSharper disable once UnusedMember.Global
    public class OrderedSelectable : Selectable {
        public int Sort { get; }

        public OrderedSelectable(Selectable selectable, int sort) {
            Guid = selectable.Guid;
            Name = selectable.Name;
            Sort = sort;
        }
    }
}
