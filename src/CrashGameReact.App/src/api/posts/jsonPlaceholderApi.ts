import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'

import { type PostDTO } from '../../models/Post'

export const jsonPlaceholderApi = createApi({
  baseQuery: fetchBaseQuery({
    baseUrl: 'https://jsonplaceholder.typicode.com'
  }),
  reducerPath: 'jsonPlaceholderApi',
  endpoints: (builder) => ({
    getPosts: builder.query<PostDTO[], null>({
      query: () => ({
        url: `/posts`
      })
    })
  })
})

export const { useGetPostsQuery } = jsonPlaceholderApi
