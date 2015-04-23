//Original Sprite shot Code
//if (sprite.Magic > 0)
//{
//    //BULLET SPEED SHOT
//    if (shot < maxbullets)
//    {
//        direction(keys);
//        if (keys.IsKeyDown(Keys.C) && oldkeys.IsKeyUp(Keys.C))
//        {
//            bullets[shot].RealX = speedx;
//            bullets[shot].RealY = speedy;
//            FireBullet(gameTime);
//            sprite.Magic -= 10;
//            shot++;
//        }
//    }
//}

//CHECKS FIRING SPEED AND DIRECTION FOR PLAYER
//public void direction(KeyboardState keys)
//        {
//            if (sprite.Left)
//            {
//                speedx = -4;
//                if (keys.IsKeyDown(Keys.Down))
//                    speedy = 4;
//                if (keys.IsKeyDown(Keys.Up))
//                    speedy = -4;
//                if (keys.IsKeyUp(Keys.Up) && keys.IsKeyUp(Keys.Down))
//                    speedy = 0;
//            }
//            if (sprite.Up)
//            {
//                speedy = -4;
//                if (keys.IsKeyDown(Keys.Right))
//                    speedx = 4;
//                if (keys.IsKeyDown(Keys.Left))
//                    speedx = -4;
//                if (keys.IsKeyUp(Keys.Left) && keys.IsKeyUp(Keys.Right))
//                    speedx = 0;
//            }
//            if (sprite.Right)
//            {
//                speedx = 4;
//                if (keys.IsKeyDown(Keys.Up))
//                    speedy = -4;
//                else if (keys.IsKeyDown(Keys.Down))
//                    speedy = 4;
//                else speedy = 0;
//            }
//            if (sprite.Down)
//            {
//                speedy = 4;
//                if (keys.IsKeyDown(Keys.Left))
//                    speedx = -4;
//                else if (keys.IsKeyDown(Keys.Right))
//                    speedx = 4;
//                else speedx = 0;
//            }
//        }

////FIRES BULLETS
//protected void FireBullet(GameTime gametime)
//{
//    // Find and fire a free bullet
//    if (!bullets[shot].Is
//    {
//        bullets[shot].Fire((int)sprite.Position.X, (int)sprite.Position.Y);
//    }
//}

////Item generator
//if (counter > 120)
//{
//    int next = generator.Next(0, 3);
//    Item tempItem = new Item();
//    if (next == 0)
//        tempItem = new SmallHealthPotion();
//    else if (next == 1)
//        tempItem = new HealthPotion();
//    else if (next == 2)
//        tempItem = new LargeHealthPotion();
//    else
//        tempItem = new manaPotion();
//    int testX = generator.Next(Game1.camerax, Game1.camerax + 800);
//    int testY = generator.Next(Game1.cameray, Game1.cameray + 480);
//    while (calculateCollision(new Rectangle(testX, testY, tempItem.defaultWidth, tempItem.defaultHeight)))
//    {
//        testX = generator.Next(Game1.camerax, Game1.camerax + 800);
//        testY = generator.Next(Game1.cameray, Game1.cameray + 480);
//    }

//    if (tempItem.GetType() == typeof(SmallHealthPotion))
//        pickupitems.Add(new SmallHealthPotion(testX,testY));
//    if (tempItem.GetType() == typeof(HealthPotion))
//        pickupitems.Add(new HealthPotion(testX, testY));
//    if (tempItem.GetType() == typeof(LargeHealthPotion))
//        pickupitems.Add(new LargeHealthPotion(testX, testY));
//    if (tempItem.GetType() == typeof(manaPotion))
//        pickupitems.Add(new manaPotion(testX, testY));
//    counter = 0;
//}
//counter++;