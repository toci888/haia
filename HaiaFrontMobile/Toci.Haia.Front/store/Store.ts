import { combineReducers, configureStore } from '@reduxjs/toolkit';
import { useDispatch, useSelector } from 'react-redux';
import { userReducer } from './user/userSlice';

const rootReducer = combineReducers({
  user: userReducer,
});

export const setupStore = (preloadedState: any) => {
  return configureStore({
    reducer: rootReducer,
    preloadedState,
  });
};

export type RootState = ReturnType<typeof rootReducer>;
export type AppStore = ReturnType<typeof setupStore>;
export type appDispatchType = AppStore['dispatch'];

export const appDispatch = useDispatch.withTypes<appDispatchType>();
export const appSelector = useSelector.withTypes<RootState>();






