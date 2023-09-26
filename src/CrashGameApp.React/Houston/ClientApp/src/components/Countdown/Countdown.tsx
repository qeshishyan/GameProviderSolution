import React, { type FC, useEffect, useState } from 'react'

import classes from './Countdown.module.scss'

export interface CountdownProps {
  callback?: () => void
  isStarted?: boolean
  countDownStartNumber?: number
}

const Countdown: FC<CountdownProps> = ({
  callback = () => {},
  isStarted = false,
  countDownStartNumber = 3
}) => {
  const [countdown, setCountdown] = useState(-1)

  useEffect(() => {
    if (!isStarted) return
    for (let i = 0; i <= countDownStartNumber; i++) {
      setTimeout(() => {
        setCountdown(countDownStartNumber - i)
        if (i === countDownStartNumber) {
          callback()
          setTimeout(() => {
            setCountdown(-1)
          }, 1000)
        }
      }, 1000 * (i + 1))
    }
  }, [isStarted, callback, countDownStartNumber])

  return (
    <div className={classes.roundTimer}>
      <span style={{ opacity: countdown === -1 ? 0 : 1 }}>
        {countdown === 0 ? 'Go!' : countdown}
      </span>
    </div>
  )
}

export default Countdown
