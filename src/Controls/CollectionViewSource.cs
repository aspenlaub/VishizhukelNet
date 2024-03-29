﻿using System;
using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;

public class CollectionViewSource : ICollectionViewSource {
    public Type EntityType { get; set; }

    public IList<ICollectionViewSourceEntity> Items { get; set; } = new List<ICollectionViewSourceEntity>();
}