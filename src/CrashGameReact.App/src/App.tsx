import React from 'react'
import { Provider } from 'react-redux'
import { Navigate, Route, BrowserRouter as Router, Routes } from 'react-router-dom'

import './App.css'
import { store } from './api/store'
import { routes } from './routes'
import './styles/global.css'

const App = (): JSX.Element => (
  <Provider store={store}>
    <Router>
      <Routes>
        {routes.map((route) => (
          <Route element={<route.component />} path={route.path} key={route.path} />
        ))}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </Router>
  </Provider>
)
export default App
