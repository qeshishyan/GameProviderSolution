import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'

import { login, logout, security } from '../paths'

export const securityApi = createApi({
  baseQuery: fetchBaseQuery({ baseUrl: security }),
  reducerPath: 'securityApi',
  endpoints: (builder) => ({
    logout: builder.mutation({
      query: () => ({
        url: logout,
        method: 'POST',
        responseHandler: 'text'
      })
    }),
    login: builder.mutation({
      query: (values) => ({
        body: values,
        url: login,
        method: 'POST',
        responseHandler: 'text'
      })
    })
  })
})

export const { useLogoutMutation, useLoginMutation } = securityApi
