import React, { type FC, type PropsWithChildren, useEffect, useState } from 'react'

import { HoustonLogo } from '../HoustonLogo/HoustonLogo'
import classes from './ApplicationBackground.module.scss'

export interface ApplicationBackgroundProps {
  isLobby?: boolean
  backgroundMovingTime?: number // seconds
}

export const ApplicationBackground: FC<
  PropsWithChildren<ApplicationBackgroundProps>
> = ({ isLobby = false, backgroundMovingTime = 0, children }) => {
  const [isBackgroundMoving, setIsBackgroundMoving] = useState(false)

  const [dynamicBgClasses, setDynamicBgClasses] = useState([classes.desktopWrapper])

  const addDynamicClass = (newClass: string) => {
    if (!dynamicBgClasses.includes(newClass)) {
      setDynamicBgClasses([...dynamicBgClasses, newClass])
    }
  }

  useEffect(() => {
    if (isLobby || backgroundMovingTime === 0) {
      setDynamicBgClasses([classes.desktopWrapper])
      setIsBackgroundMoving(false)
      return
    }
    const isMoving = !(backgroundMovingTime === 0)
    if (isMoving) {
      addDynamicClass(classes.gameActionWrapper)
    }
    setIsBackgroundMoving(isMoving)
    setTimeout(() => {
      addDynamicClass(classes.gameActionWrapper)
      addDynamicClass(classes.gameActionEndWrapper)
      setTimeout(() => {
        setIsBackgroundMoving(false)
      }, 3000) // 3s to darken bg
    }, backgroundMovingTime * 1000)
  }, [backgroundMovingTime, isBackgroundMoving])

  return (
    <div>
      {isLobby ? (
        <div className={classes.desktopWrapper}>
          <div className={classes.logoMain}>
            <HoustonLogo />
          </div>
          {children}
        </div>
      ) : (
        <>
          <div className={dynamicBgClasses.join(' ')}>{children}</div>
          <div
            className={
              isBackgroundMoving
                ? classes.gameActionEarthFooter
                : classes.earthFooter
            }
          />
        </>
      )}
    </div>
  )
}
