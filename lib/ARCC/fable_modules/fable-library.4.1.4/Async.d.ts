import { Continuation, Continuations } from "./AsyncBuilder.js";
import { CancellationToken } from "./AsyncBuilder.js";
import { IAsync } from "./AsyncBuilder.js";
import { IAsyncContext } from "./AsyncBuilder.js";
import { FSharpChoice$2_$union } from "./Choice.js";
export declare class Async<_T> {
}
export declare function makeAsync<T>(body: IAsync<T>): IAsync<T>;
export declare function invoke<T>(computation: IAsync<T>, ctx: IAsyncContext<T>): void;
export declare function callThenInvoke<T, U>(ctx: IAsyncContext<T>, result1: U, part2: (x: U) => IAsync<T>): void;
export declare function bind<T, U>(ctx: IAsyncContext<T>, part1: IAsync<U>, part2: (x: U) => IAsync<T>): void;
export declare function createCancellationToken(arg?: boolean | number): CancellationToken;
export declare function cancel(token: CancellationToken): void;
export declare function cancelAfter(token: CancellationToken, ms: number): void;
export declare function isCancellationRequested(token: CancellationToken): boolean;
export declare function throwIfCancellationRequested(token: CancellationToken): void;
export declare function startChild<T>(computation: IAsync<T>, ms?: number): IAsync<IAsync<T>>;
export declare function awaitPromise<T>(p: Promise<T>): (ctx: IAsyncContext<T>) => void;
export declare function cancellationToken(): (ctx: IAsyncContext<CancellationToken>) => void;
export declare const defaultCancellationToken: CancellationToken;
export declare function catchAsync<T>(work: IAsync<T>): (ctx: IAsyncContext<FSharpChoice$2_$union<T, Error>>) => void;
export declare function fromContinuations<T>(f: (conts: Continuations<T>) => void): (ctx: IAsyncContext<T>) => void;
export declare function ignore<T>(computation: IAsync<T>): (ctx: IAsyncContext<undefined>) => void;
export declare function parallel<T>(computations: Iterable<IAsync<T>>): (ctx: IAsyncContext<Awaited<T>[]>) => void;
export declare function sequential<T>(computations: Iterable<IAsync<T>>): (ctx: IAsyncContext<T[]>) => void;
export declare function sleep(millisecondsDueTime: number): (ctx: IAsyncContext<void>) => void;
export declare function runSynchronously(): never;
export declare function start<T>(computation: IAsync<T>, cancellationToken?: CancellationToken): void;
export declare function startImmediate<T>(computation: IAsync<T>, cancellationToken?: CancellationToken): void;
export declare function startWithContinuations<T>(computation: IAsync<T>, continuation?: Continuation<T> | CancellationToken, exceptionContinuation?: Continuation<any>, cancellationContinuation?: Continuation<any>, cancelToken?: CancellationToken): void;
export declare function startAsPromise<T>(computation: IAsync<T>, cancellationToken?: CancellationToken): Promise<T>;
export default Async;