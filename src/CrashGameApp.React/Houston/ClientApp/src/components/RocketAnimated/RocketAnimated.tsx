import useSound from 'use-sound'

import React, { type FC, useEffect, useState } from 'react'
import { animated, useSpring } from 'react-spring'

import endExplosion from '../../assets/images/rocket-animated/explosion.svg'
import fire from '../../assets/images/rocket-animated/fireTorch.gif'
import rocket from '../../assets/images/rocket-animated/rocket.png'
import smoke from '../../assets/images/rocket-animated/smoke.gif'
import weHaveAProblem from '../../assets/images/rocket-animated/we-have-a-problem.svg'
import explodeSound from '../../assets/sounds/explode.mp3'
import flyingSound from '../../assets/sounds/flying.mp3'
import startSound from '../../assets/sounds/start.mp3'
import classes from './RocketAnimated.module.scss'

export interface RocketAnimatedProps {
  isAction?: boolean
  handleClick?: () => void
  onLaunchAnimationEnd?: () => void
  isLobby?: boolean
  isRocketExplosion?: boolean
}

export const RocketAnimated: FC<RocketAnimatedProps> = ({
  isAction = false,
  handleClick = () => {},
  onLaunchAnimationEnd = () => {},
  isLobby = false,
  isRocketExplosion = false
}) => {
  const [playStartSound, { stop: stopStartSound }] = useSound(startSound)
  const [playStartExplode, { stop: stopExplodeSound }] = useSound(explodeSound)
  const [playFlyingSound, { stop: stopFlyingSound }] = useSound(flyingSound, {
    loop: !isLobby,
    playbackRate: !isLobby ? 1 : 3
  })

  const applicationHeight =
    document.documentElement?.clientHeight !== 0
      ? document.documentElement.clientHeight
      : 0

  const [isSurfing, setIsSurfing] = useState(false)

  useEffect(() => {
    return () => {
      stopStartSound()
      stopExplodeSound()
      stopFlyingSound()
    }
  }, [])

  useEffect(() => {
    if (isSurfing) {
      playFlyingSound()
      stopExplodeSound()
    }
    if (isAction && !isSurfing) {
      playStartSound()
      stopExplodeSound()
    }
    if (isRocketExplosion) {
      setIsSurfing(false)
      stopFlyingSound()
      stopStartSound()
      playStartExplode()
    }
  }, [isRocketExplosion, isSurfing, isAction])

  const onLaunchEnd = () => {
    if (isSurfing) {
      onLaunchAnimationEnd()
      return
    }
    if (isAction) {
      setIsSurfing(true)
    }
  }

  // isLobby ? applicationHeight should be enough for rocket to leave the screen considering rocket div is 30% from the bottom
  // else ? calculates height enough for rocket to stay inside game container knowing rocket has 50% of container space above it
  const rocketMaxHeightOnScreen = isLobby
    ? applicationHeight
    : 0.2 * applicationHeight
  // calculates how low will rocket get before takeoff
  const rocketPrepareForTakeoffHeight = 0.05 * applicationHeight

  const rocketSurfStyles = useSpring({
    from: {
      transform: `translateY(${
        isSurfing ? rocketPrepareForTakeoffHeight + 'px' : '0px'
      })`
    },
    to: {
      transform: `translateY(${
        isSurfing
          ? '-' + rocketMaxHeightOnScreen + 'px'
          : isAction
          ? rocketPrepareForTakeoffHeight + 'px'
          : '0px'
      })`
    },
    config: { mass: 6, duration: 1000 },
    onRest: () => {
      onLaunchEnd()
    }
  })

  const startExplosionStyles = useSpring({
    from: { opacity: 0, transform: 'translateY(0px)' },
    to: [
      {
        opacity: isAction && !isSurfing ? 1 : 0,
        transform: `translateY(${isAction ? '50px' : '0px'})`
      },
      { opacity: 0 }
    ],
    config: { mass: 6, duration: 400 }
  })

  const endExplosionStyles = useSpring({
    from: { opacity: 0 },
    to: [{ opacity: isRocketExplosion ? 1 : 0 }, { opacity: 0 }],
    config: { mass: 6, duration: 1000 }
  })

  return (
    <div className={classes.rocketContainer}>
      {!isLobby ? (
        <>
          <animated.div
            className={classes.weHaveAProblem}
            style={endExplosionStyles}>
            <img src={weHaveAProblem} alt="" />
          </animated.div>
          <animated.div className={classes.endExplosion} style={endExplosionStyles}>
            <img src={endExplosion} alt="" />
          </animated.div>
          <animated.div
            className={classes.endExplosion + ' ' + classes.endExplosionRight}
            style={endExplosionStyles}>
            <img src={endExplosion} alt="" />
          </animated.div>
          <animated.div
            className={classes.endExplosion + ' ' + classes.endExplosionLeft}
            style={endExplosionStyles}>
            <img src={endExplosion} alt="" />
          </animated.div>
        </>
      ) : null}

      <animated.div
        className={
          isRocketExplosion
            ? classes.rocketMain + ' ' + classes.transparent
            : classes.rocketMain
        }
        style={rocketSurfStyles}>
        <img src={rocket} alt="HeyHouston Rocket" onClick={handleClick} />
      </animated.div>

      {isAction ? (
        <animated.div
          className={
            isRocketExplosion
              ? classes.fireMain + ' ' + classes.transparent
              : classes.fireMain
          }
          style={rocketSurfStyles}>
          <img
            src={fire}
            alt="Fire Rocket"
            onClick={handleClick}
            style={{ transform: 'rotate(90deg)' }}
          />
        </animated.div>
      ) : null}

      {!isLobby ? (
        <animated.div
          className={classes.startExplosion}
          style={startExplosionStyles}>
          <img src={smoke} alt="" />
        </animated.div>
      ) : null}
    </div>
  )
}
