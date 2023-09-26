import React, { type ReactNode } from 'react'
import { animated } from 'react-spring'

import classes from './HoverComponent.module.scss'
import { useHover } from './useHover'

interface Props {
  children: ReactNode
  autoBet: string
  className?: any
  onClick?: () => void
}

export const HoverComponent = ({ children, autoBet, className, onClick }: Props) => {
  const { hoverOver, hoverLeave, hoverStyles } = useHover()
  let style = classes.hoverComponent
  if (className) {
    style += className
  }
  return (
    <animated.div
      onMouseOver={hoverOver}
      onMouseLeave={hoverLeave}
      key={autoBet}
      className={style}
      style={hoverStyles}
      onClick={onClick}>
      {children}
    </animated.div>
  )
}
