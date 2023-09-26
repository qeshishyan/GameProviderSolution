import React, { type FC, useEffect, useRef, useState } from 'react'

import classes from './ProgressBar.module.scss'

export interface ProgressBarProps {
  isLoading?: boolean
  onEnd?: () => void
  animationTime?: number
}

export const ProgressBar: FC<ProgressBarProps> = ({
  isLoading = false,
  animationTime = 5,
  onEnd = () => {}
}) => {
  const [value, setValue] = useState(0)

  useEffect(() => {
    if (isLoading) setValue(100)
  }, [isLoading])

  const progressBar = useRef<HTMLDivElement>(null)
  useEffect(() => {
    progressBar.current?.addEventListener('transitionend', onEnd)
  }, [onEnd])

  return (
    <div className={classes.progress}>
      <div
        className={classes.progressValue}
        style={{
          transition: `width ${animationTime}s ease 0s`,
          width: `${value}%`
        }}
        ref={progressBar}
      />
    </div>
  )
}
