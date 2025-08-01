# 事件系统框架

## 目录架构
 - Base基础的事件系统代码
   - EventBus 实现 IEventBus接口 作为事件的转发器，存储所有订阅的事件以及进行事件的转发
   - EventSubscribeComponent 实现IEventComponent接口，作为实际上的事件注册和取消订阅的工具，同时还负责事件广播，存储了由该组件注册的事件，方便进行取消订阅处理
   - Mono 和unity Mono相关的组件
     - GameObjectDestroyedEvent 一个mono生命周期相关的事件，用于game object被销毁时调用
     - MonoEventSubComponent 绑定在物体上的mono组件，内置一些基础的调用，通过调用内置的EventSubscribeComponent 来实现事件处理
 - Excample 一些例子
   - UniTest 一些简单的测试
     - RedGreen 实现了一个红绿灯的功能，Light负责管理红绿灯的数据，当红绿灯变化时发送事件，改变ui，并且让car输出一个信息
     - TickTock 简单的测试，实际上没什么功能，可以用来做压力测试
 - GlobalEventBus 作为一个静态类提供一个全局的单例转发器，EventBus，用来作为全局事件的转发中心
## 简述
这套架构中间使用了一些转换的小巧思，因此事件的数据类可以是struct或者是class，即使使用struct也不会使用拆装箱。但是如果事件数据中包含引用类型推荐使用class而不是struck

这个框架只是事件系统的一种实现逻辑，外部调用均通过接口参与，而不是直接调用，因此本系统虽然可能会在其它的框架中使用，但是并不会影响到其它框架，只需要实现接口中的内容即可无缝衔接