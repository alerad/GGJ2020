using System;
using QuarkFramework;
using UniRx;

public interface IQuarkAny {
    
}

public interface IQuarkSystem : IQuarkAny{
}


public interface IQuarkComponent : IQuarkAny {
}

public interface IQuarkNodeSystem: IQuarkSystem {
    IObservable<QuarkNode> stream();
}

