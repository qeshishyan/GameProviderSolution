import React, { type FC, useEffect, useState } from 'react'

import BetCoefficientScale from '../BetCoefficientScale/BetCoefficientScale'
import Countdown from '../Countdown/Countdown'
import PlayablePanel from '../PlayablePanel/PlayablePanel'
import { RocketAnimated } from '../RocketAnimated/RocketAnimated'
import classes from './GameContainer.module.scss'

export interface GameContainerProps {
  handleBackgroundAnimation?: (backgroundMovingTime: number) => void
}

const GameContainer: FC<GameContainerProps> = ({
  handleBackgroundAnimation = () => {}
}) => {
  const [isCountdown, setIsCountdown] = useState(false)
  const [isRocketAction, setIsRocketAction] = useState(false)
  const [isRocketLaunched, setIsRocketLaunched] = useState(false)
  const [isRocketExplosion, setIsRocketExplosion] = useState(false)
  const [flightDuration, setFlightDuration] = useState(0)

  // todo сколько секунд будет лететь ракета:
  useEffect(() => {
    if (flightDuration !== 0) return
    const min = 1
    const max = 100
    let durationSeconds
    const rand = min + Math.random() * (max - min)
    if (rand < 50) durationSeconds = 1 + Math.random()
    else durationSeconds = 1 + Math.random() * 100
    console.log(durationSeconds)
    setFlightDuration(durationSeconds)
  })

  const handleRocketFlightDuration = () => {
    if (!isRocketAction) return
    handleBackgroundAnimation(flightDuration)
    setIsRocketLaunched(true)
    setTimeout(() => {
      setIsRocketExplosion(true)
      setTimeout(() => {
        setIsRocketLaunched(false)
        setIsRocketExplosion(false)
        setIsRocketAction(false)
        setFlightDuration(0)
        handleBackgroundAnimation(0)
      }, 3000) // 3s to darken ApplicationBackground
    }, flightDuration * 1000)
  }

  return (
    <>
      <div className={classes.gamePlayableContainer}>
        <Countdown
          callback={() => {
            setIsRocketAction(true)
            setIsCountdown(false)
          }}
          isStarted={isCountdown}
        />

        <div className={classes.rocketGame}>
          <RocketAnimated
            isAction={isRocketAction && !isRocketExplosion}
            onLaunchAnimationEnd={() => {
              handleRocketFlightDuration()
            }}
            isRocketExplosion={isRocketExplosion}
          />
        </div>

        <div className={classes.playablePanelContainerWrapper}>
          <PlayablePanel
            handleStartClick={() => {
              setIsCountdown(true)
            }}
            isRoundStarted={isRocketAction}
          />
        </div>
      </div>

      <div className={classes.gameCoefficientScale}>
        <BetCoefficientScale
          isAction={isRocketLaunched}
          coefficient={flightDuration}
        />
      </div>
    </>
  )
}

export default GameContainer
