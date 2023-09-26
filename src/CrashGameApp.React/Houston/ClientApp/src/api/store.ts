import {
  type ThunkMiddleware,
  combineReducers,
  configureStore
} from '@reduxjs/toolkit'

import { type TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux'

import { jsonPlaceholderApi } from './posts/jsonPlaceholderApi'
import { rtkQueryHandler } from './rtkQueryHandler'
import { securityApi } from './security/securityApi'

const apis = [jsonPlaceholderApi, securityApi]

const reducers = {
  // Если приложение использует slice, тогда сюда нужно вставить в формате:
  // [themeSlice.name]: themeSlice.reducer
}

const createRootReducer = () =>
  combineReducers(
    apis.reduce(
      (storeReducers, api) => ({
        ...storeReducers,
        [api.reducerPath]: api.reducer
      }),
      {
        ...reducers
      }
    )
  )

export const store = configureStore({
  reducer: createRootReducer(),
  middleware: (getDefaultMiddleware) => [
    ...getDefaultMiddleware(),
    jsonPlaceholderApi.middleware as ThunkMiddleware,
    rtkQueryHandler
  ],
  devTools: process.env.NODE_ENV !== 'production'
})

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch

export const useAppDispatch: () => AppDispatch = useDispatch
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector
